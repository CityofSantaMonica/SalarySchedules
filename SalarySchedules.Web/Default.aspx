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
            <div id="data"></div>
        </div>
    </div>    
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContentPlaceHolder">
    <script src="js/masonry.pkgd.min.js"></script>
    <script src="js/salarySchedules.js"></script>
</asp:Content>