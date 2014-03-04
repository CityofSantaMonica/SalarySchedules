function SalaryScheduleApp() {
    var app = this;

    app.loadScheduleData = function (filePath, callback) {
        var args = JSON.stringify({ "file": filePath });

        $.ajax({
            type: "POST",
            data: args,
            dataType: "json",
            url: "SalaryScheduleService.asmx/GetSchedule",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d) data = data.d;
                callback(data);
            },
            error: function (e) {
                console.log(e);
            },
        });

        return false;
    };
};

var templates = {
    "jobClasses": $("<div />"),

    "jobClass": $("<div />").addClass("jobClass")
                            .append($("<h4 />"))
                            .append($("<span />").addClass("code"))
                            .append($("<span />").addClass("grade"))
                            .append($("<span />").addClass("bargainingUnit"))
                            .append($("<div />").addClass("steps")),

    "steps": $("<table />").addClass("table table-striped table-bordered"),

    "step": $("<tr>").append($("<td />").addClass("hourly"))
                     .append($("<td />").addClass("biweekly"))
                     .append($("<td />").addClass("monthly"))
                     .append($("<td />").addClass("annual"))
};

$(function () {
    var $target = $("#data");

    $("#submit").on("click", function (e) {
        var filePath = $("#YearSelect").val(),
            app = new SalaryScheduleApp();

        app.loadScheduleData(filePath, function (data) {
            $target.empty();
            $target.append($("<h3 />").text("Bargaining Units"));

            var units = $("<table />").addClass("table table-striped table-bordered");

            $.each(data.BargainingUnits, function (i, bu) {
                var unit = $("<tr />").append($("<td />").text(bu.Code)).append($("<td />").text(bu.Name));
                units.append(unit);
            })

            $target.append($("<div />").addClass("table-responsive").append(units));

            $target.append($("<h3 />").text("Job Classes"));

            var $jobClasses = templates.jobClasses.clone();

            $.each(data.JobClasses, function (i, jobClassData) {
                var $jobClass = templates.jobClass.clone();

                $("h4", $jobClass).text(jobClassData.Title);
                $(".code", $jobClass).text(jobClassData.Code);
                $(".grade", $jobClass).text(jobClassData.Grade);
                $(".bargainingUnit", $jobClass).text(jobClassData.BargainingUnit ? jobClassData.BargainingUnit.Code : "N/A");

                var $steps = templates.steps.clone();

                $.each(jobClassData.Steps, function (j, stepData) {
                    var $step = templates.step.clone();
                    $(".hourly", $step).text(stepData.HourlyRate);
                    $(".biweekly", $step).text(stepData.BiWeeklyRate);
                    $(".monthly", $step).text(stepData.MonthlyRate);
                    $(".annual", $step).text(stepData.AnnualRate);
                    $steps.append($step);
                });

                $(".steps", $jobClass).append($steps);

                $jobClasses.append($jobClass);
            });

            $target.append($jobClasses);
        });

        e.preventDefault();
    });
});