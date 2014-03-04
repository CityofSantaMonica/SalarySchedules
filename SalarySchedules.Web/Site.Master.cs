using System;

namespace SalarySchedules.Web
{
    public partial class Site_Master : System.Web.UI.MasterPage
    {
        protected void AddFormAttribute(string key, string value)
        {
            Form.Attributes.Add(key, value);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}