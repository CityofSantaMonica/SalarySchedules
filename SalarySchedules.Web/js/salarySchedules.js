function SalaryScheduleApp($target) {

    //knockout view models

    function salaryScheduleViewModel() {
        var self = this,
            $allJobClasses = $([]);

        self.BargainingUnits = ko.observableArray().extend({ rateLimit: 50 });
        self.FiscalYear = ko.observable();        
        self.JobClasses = ko.observableArray().extend({ rateLimit: 50 });
        self.ReportRunDate = ko.observable();
        
        self.FiscalYearLabel = ko.computed(function () {
            return self.FiscalYear() ? "FY " + self.FiscalYear().ShortSpanCode : "No Fiscal Year";
        });
        
        self.FilterByBargainingUnit = function (bu) {
            var filteredClasses = $allJobClasses.filter(function () {
                return this.BargainingUnit && this.BargainingUnit.Code === bu.Code;
            });
            self.JobClasses(filteredClasses);
        };
        
        self.Initialize = function (data) {
            $allJobClasses = $($.extend(true, [], data.JobClasses));

            self.ReportRunDate(data.ReportRunDate);
            self.FiscalYear(data.FiscalYear);
            
            $.each(data.BargainingUnits, function (i, e) {
                self.BargainingUnits.push(e);
            });

            $.each(data.JobClasses, function (i, e) {
                self.JobClasses.push(new jobClassViewModel(e));
            });
        };
    };

    function jobClassViewModel(jobClassData) {
        var self = this;
        var mapping = {
            "Steps": {
                create: function (options) {
                    return new jobClassStepViewModel(options.data);
                }
            }
        };

        ko.mapping.fromJS(jobClassData, mapping, self);
    };

    function jobClassStepViewModel(jobClassStepData) {
        var self = this;
        ko.mapping.fromJS(jobClassStepData, {}, self);
    };
    
    function loadComplete(data, callback) {
        rebind(data);
        //applyMasonry();
        if (callback)
            callback();
        $target.fadeIn();
    };

    var rebind = function (data) {
        viewModel.Initialize(data);
        
        if (!bound) {
            ko.applyBindings(viewModel);
            bound = true;
        }
    };

    var applyMasonry = function () {
        var $jobClassesTarget = $("div.jobClasses", $target);

        $jobClassesTarget.masonry("destroy");

        $jobClassesTarget.prepend(
            $("<div />").addClass("sizer")
        );

        $jobClassesTarget.masonry({
            itemSelector: "div.jobClass"
        });
    };

    var loadedData = {},
        viewModel = new salaryScheduleViewModel(),
        bound = false;

    //the public interface
    
    this.loadScheduleData = function (filePath, callback) {
        $target.fadeOut();

        if (loadedData[filePath]) {
            loadComplete(loadedData[filePath], callback);
        }
        else {
            var args = JSON.stringify({ "file": filePath });

            $.ajax({
                type: "POST",
                data: args,
                dataType: "json",
                url: "SalaryScheduleService.asmx/GetSchedule",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d) data = data.d;
                    loadedData[filePath] = data;
                    loadComplete(data, callback);
                },
                error: function (e) {
                    console.log(e);
                },
            });
        }

        return false;
    };
};

$(function () {
    var $dataContainer = $("#data"),
        $loader = $("#loader"),
        $jobCarets = $(".jobClass h4 .caret");
        app = new SalaryScheduleApp($dataContainer);
    
    $("#submit").on("click", function (e) {
        $loader.show();
        e.preventDefault();
        var filePath = $("#YearSelect").val();
        app.loadScheduleData(filePath, function () {
            window.setTimeout(function () { $loader.hide(); }, 250);            
        });
    });

    $jobCarets.parent().on("click", function () {
        $(this).next(".body").slideToggle();
    });
});