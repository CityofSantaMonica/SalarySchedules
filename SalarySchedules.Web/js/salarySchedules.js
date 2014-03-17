function SalaryScheduleApp($target) {

    //knockout view models

    function salaryScheduleViewModel(data) {
        var self = this;
        var allJobClasses = $.extend(true, [], data.JobClasses);

        self.ReportRunDate = ko.observable(data.ReportRunDate);
        self.FiscalYear = ko.observable(data.FiscalYear);
        self.FiscalYearLabel = ko.computed(function () {
            return "FY " + self.FiscalYear().ShortSpanCode;
        }, self);

        self.BargainingUnits = ko.observableArray();

        self.JobClasses = ko.observableArray();
        self.FilterByBargainingUnit = function (bu) {
            var filteredClasses = $(allJobClasses).filter(function () {
                return !(this.BargainingUnit === undefined || this.BargainingUnit === null)
                               && (this.BargainingUnit.Code === bu.Code);
            });
            self.JobClasses(filteredClasses);
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

        self.CodeLabel = ko.computed(function () {
            return "Code: " + self.Code();
        });
        self.GradeLabel = ko.computed(function () {
            return "Grade: " + self.Grade();
        });
        self.BargainingUnitLabel = ko.computed(function () {
            var code = "BU: ";
            if (self.BargainingUnit && self.BargainingUnit.Code) {
                return code + self.BargainingUnit.Code();
            }
            else {
                return code + "N/A";
            }
        });
    };

    function jobClassStepViewModel(jobClassStepData) {
        var self = this;
        ko.mapping.fromJS(jobClassStepData, {}, self);

        self.HourlyLabel = ko.computed(function () {
            return self.HourlyRate().toFixed(2);
        });

        self.BiWeeklyLabel = ko.computed(function () {
            return self.BiWeeklyRate().toFixed(2);
        });

        self.MonthlyLabel = ko.computed(function () {
            return self.MonthlyRate().toFixed(2);
        });

        self.AnnualLabel = ko.computed(function () {
            return self.AnnualRate().toFixed(2);
        });
    };
    
    function loadComplete(data) {
        rebind(data);
        applyMasonry();
    };

    var rebind = function (data) {
        $target.empty();

        var viewModel = new salaryScheduleViewModel(data);

        viewModel.BargainingUnits.extend({ rateLimit: 50 });

        viewModel.JobClasses.extend({ rateLimit: 50 });

        $.each(data.BargainingUnits, function (i, e) {
            viewModel.BargainingUnits.push(e);
        });
        
        $.each(data.JobClasses, function (i, e) {
            viewModel.JobClasses.push(new jobClassViewModel(e));
        });

        ko.applyBindings(viewModel);
    };

    var applyMasonry = function () {
        $target.fadeIn();

        //var $jobClassesTarget = $("div.jobClasses", $target);

        //$jobClassesTarget.masonry("destroy");

        //$jobClassesTarget.prepend(
        //    $("<div />").addClass("sizer")
        //);

        //$jobClassesTarget.masonry({
        //    itemSelector: "div.jobClass"
        //});
    };

    var loadedData = {};

    //the public interface
    
    this.loadScheduleData = function (filePath) {
        $target.fadeOut();

        if (loadedData[filePath]) {
            loadComplete(loadedData[filePath]);
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
                    loadComplete(data);
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
    var $data = $("#data"),
        app = new SalaryScheduleApp($data);

    $("#submit").on("click", function (e) {
        e.preventDefault();
        var filePath = $("#YearSelect").val();
        app.loadScheduleData(filePath);
    });

    $data.fadeOut();
});