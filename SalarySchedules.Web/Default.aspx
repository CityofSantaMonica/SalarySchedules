<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SalarySchedules.Web.Default" MasterPageFile="~/Site.Master" %>
<%@ MasterType TypeName="SalarySchedules.Web.Site_Master"%>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
    <div class="row">
        <div class="col-lg-12">
            <h1>City of Santa Monica Salary Schedules</h1>
            <p>The City of Santa Monica publishes <a href="http://www.smgov.net/Departments/HR/Employees/Employees.aspx" target="_blank">employee salary schedules</a> in PDF format each fiscal year.</p>
            <p>The aim of this project is to convert human-friendly PDF into machine-friendly JSON.</p>
            <p>This project uses the <a href="http://www.gnu.org/licenses/agpl.html" target="_blank">AGPL</a> Nuget package <a href="http://sourceforge.net/projects/itextsharp/" target="_blank">iTextSharp</a> to read data as text from PDF documents.</p>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <h2>The Data</h2>
            
            <asp:Label ID="YearLabel" runat="server" Text="Select a fiscal year: "></asp:Label>            
            <asp:DropDownList ID="YearSelect" runat="server" AutoPostBack="false" ClientIDMode="Static"></asp:DropDownList>
            <button id="submit">Load Data</button>

            <div id="data">
                <h3 data-bind="text: FiscalYearLabel"></h3>
                
                <h3>Bargaining Units</h3>                
                <table class="table table-striped table-bordered allBargainingUnits">
                    <thead>
                        <tr>
                            <th>Code</th>
                            <th>Name</th>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: BargainingUnits">
                        <tr>
                            <td class="code"><a href="#" data-bind="click: $parent.FilterByBargainingUnit"><span data-bind="    text: Code"></span></a></td>
                            <td class="name" data-bind="text: Name"></td>
                        </tr>
                    </tbody>
                </table>
                
                <h3 class="jobClasses">Job Classes</h3>                
                <input type="text" id="search" placeholder="Search Job Classes" />                
                <div class="jobClasses" data-bind="foreach: JobClasses">
                    <div class="jobClass">
                        <h4 data-bind="text: Title"></h4>
                        <div class="description">
                            <span class="code" data-bind="text: CodeLabel"></span>
                            <span class="grade" data-bind="text: GradeLabel"></span>
                            <span class="bargainingUnit" data-bind="text: BargainingUnitLabel"></span>
                        </div>
                        <div class="steps">
                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th>Hourly</th>
                                        <th>BiWeekly</th>
                                        <th>Monthly</th>
                                        <th>Annually</th>
                                    </tr>
                                </thead>
                                <tbody data-bind="foreach: Steps">
                                    <tr class="step">
                                        <td class="hourly rate" data-bind="text: HourlyLabel"></td>
                                        <td class="biweekly rate" data-bind="text: BiWeeklyLabel"></td>
                                        <td class="monthly rate" data-bind="text: MonthlyLabel"></td>
                                        <td class="annual rate" data-bind="text: AnnualLabel"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>                    
                </div>
            </div>
        </div>
    </div>    
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContentPlaceHolder">
    <script src="js/masonry.pkgd.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/knockout/3.1.0/knockout-min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/knockout.mapping/2.4.1/knockout.mapping.js"></script>
    <script src="js/salarySchedules.js"></script>
</asp:Content>