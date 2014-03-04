function SalaryScheduleApp() {
    var jobClasses = [];

    this.loadScheduleData = function (filePath, callback) {
        var args = JSON.stringify({ "file": filePath });

        $.ajax({
            type: "POST",
            data: args,
            dataType: "json",
            url: "SalaryScheduleService.asmx/GetSchedule",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d) data = data.d;
                jobClasses = callback(data);
            },
            error: function (e) {
                console.log(e);
            },
        });

        return false;
    };

    this.filterJobClasses = function (filterFunc) {
        return $(jobClasses).filter(filterFunc);
    };
};

var templates = {
    "jobClasses": $("<div />").addClass("jobClasses").append($("<div />").addClass("sizer")),

    "jobClass": $("<div />").addClass("jobClass")
                            .append($("<h4 />"))
                            .append($("<div />").addClass("description")
                                .append($("<span />").addClass("code"))
                                .append($("<span />").addClass("grade"))
                                .append($("<span />").addClass("bargainingUnit")))
                            .append($("<div />").addClass("steps")),

    "table": $("<table />").addClass("table table-striped table-bordered"),

    "bargainingUnit": $("<tr />").append($("<td />").addClass("code")).append($("<td />").addClass("name")),

    "step": $("<tr>").append($("<td />").addClass("hourly rate"))
                     .append($("<td />").addClass("biweekly rate"))
                     .append($("<td />").addClass("monthly rate"))
                     .append($("<td />").addClass("annual rate"))
};

$(function () {
    var $target = $("#data");

    $("#submit").on("click", function (e) {
        var filePath = $("#YearSelect").val(),
            app = new SalaryScheduleApp();

        app.loadScheduleData(filePath, function (data) {
            var $jobClasses = rebind(data);
            allJobClasses = $jobClasses;
            return $jobClasses;
        });

        e.preventDefault();
    });

    var rebind = function (data, bu, pre) {
        $target.empty();
        $target.append($("<h3 />").text("FY " + data.FiscalYear.ShortSpanCode));
        bindBargainingUnits(data);
        var $jobClasses = bindJobClasses(data, bu, pre);
        return $jobClasses;
    };

    var bindBargainingUnits = function (data) {
        $target.append($("<h3 />").text("Bargaining Units"));

        var $units = templates.table.clone(),
            $a = $("<a />");

        $.each(data.BargainingUnits, function (i, bu) {
            var $unit = templates.bargainingUnit.clone(),
                $code = $a.clone().on("click", function () { rebind(data, bu.Code); }).attr("href", "#").text(bu.Code);

            $(".code", $unit).html($code);
            $(".name", $unit).text(bu.Name);
            $units.append($unit);
        });

        $target.append($("<div />").addClass("table-responsive").append($units));
    };

    var bindJobClasses = function (data, bu) {
        var jobClassesData = data.JobClasses;

        if (bu) {
            jobClassesData = $(data.JobClasses).filter(function (i) {
                return !(this.BargainingUnit === undefined || this.BargainingUnit === null)
                           && (this.BargainingUnit.Code === bu);
            });
        }

        $target.append($("<h3 />").text("Job Classes" + (bu ? " (" + bu + ")" : "")));

        var $jobClasses = templates.jobClasses.clone();

        $.each(jobClassesData, function (i, jobClassData) {
            var $jobClass = templates.jobClass.clone();

            $("h4", $jobClass).text(jobClassData.Title);
            $(".code", $jobClass).text("Code: " + jobClassData.Code);
            $(".grade", $jobClass).text("Grade: " + jobClassData.Grade);
            $(".bargainingUnit", $jobClass).text("BU: " + (jobClassData.BargainingUnit ? jobClassData.BargainingUnit.Code : "N/A"));

            var $steps = templates.table.clone().append($("<thead><tr><td>Hourly</td><td>BiWeekly</td><td>Monthly</td><td>Annually</td></tr></thead>"));

            $.each(jobClassData.Steps, function (j, stepData) {
                var $step = templates.step.clone();
                $(".hourly", $step).text(stepData.HourlyRate.toFixed(2));
                $(".biweekly", $step).text(stepData.BiWeeklyRate.toFixed(2));
                $(".monthly", $step).text(stepData.MonthlyRate.toFixed(2));
                $(".annual", $step).text(stepData.AnnualRate.toFixed(2));
                $steps.append($step);
            });

            $(".steps", $jobClass).append($steps);

            $jobClasses.append($jobClass);
        });

        $target.append($jobClasses);
        
        $jobClasses.masonry({
            columnWidth: ".sizer",
            itemSelector: ".jobClass",
            gutter: 5,
            isFitWidth: true
        });

        return $jobClasses;
    };
});