<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SalarySchedules.Web.Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>City of Santa Monica Salary Schedules</title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <h1>City of Santa Monica Salary Schedules</h1>
        <p>The City of Santa Monica publishes <a href="http://www.smgov.net/Departments/HR/Employees/Employees.aspx" target="_blank">employee salary schedules</a> in PDF format each fiscal year.</p>
        <p>The aim of this project is to convert human-friendly PDF into machine-friendly JSON.</p>
        <p>This project uses the <a href="http://www.gnu.org/licenses/agpl.html" target="_blank">AGPL</a> Nuget package <a href="http://sourceforge.net/projects/itextsharp/" target="_blank">iTextSharp</a> to read data as text from PDF documents.</p>

        <h2>The Data</h2>
        <asp:Label ID="YearLabel" runat="server" Text="Select a fiscal year: "></asp:Label>
        <asp:DropDownList ID="YearSelect" runat="server" AutoPostBack="false" ClientIDMode="Static"></asp:DropDownList>
        <input type="submit" id="submit" value="Load Data" />

        <div id="data"></div>
    </form>
    <script type="text/javascript">
        $(function () {
            var schedules = {},
                $fiscalYear = $("#YearSelect"),
                $submit = $("#submit"),
                $target = $("#data");

            $submit.on("click", function () {
                var schedule = load($fiscalYear.val());
                bind(schedule);
            });

            function load(file) {
                var data = { "file": file };
                
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(data),
                    dataType: "json",
                    url: "SalaryScheduleService.asmx/GetSchedule",
                    contentType: "application/json; charset=utf-8",
                    success: function (d) {
                        console.log(d);
                    },
                    error: function (e) {
                        console.log(e);
                    },
                });

                return false;
            }

            function bind(data) {

            }
        });
    </script>
</body>
</html>
