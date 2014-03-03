using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SalarySchedules.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Regex fiscalYear = new Regex(@"(\d\d)-(\d\d)");
            
            var serverPath = Server.MapPath("~/Resources");

            var files = Directory.GetFiles(serverPath)
                                 .Select(f => f.Replace(serverPath, string.Empty).Trim('\\'))
                                 .OrderByDescending(f => f);

            var dataSource = files.Select(f =>
            {
                var m = fiscalYear.Match(f);
                return new {
                    Value = f,
                    Text = String.Format("{0}/{1}", m.Groups[1], m.Groups[2])
                };
            });

            YearSelect.DataTextField = "Text";
            YearSelect.DataValueField = "Value";
            YearSelect.DataSource = dataSource;            
            YearSelect.DataBind();
        }
    }
}