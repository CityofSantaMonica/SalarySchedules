using SalarySchedules.Models;
using SalarySchedules.Parser;
using System.Web;
using System.Web.Services;

namespace SalarySchedules.Web
{
    [WebService(Namespace = "http://wwww.smgov.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class SalaryScheduleService : System.Web.Services.WebService
    {
        [WebMethod]
        public SalarySchedule GetSchedule(string file)
        {
            var serverFile = HttpContext.Current.Server.MapPath(string.Format("~/Resources/{0}", file));
            ISalaryScheduleParser parser = new CSMSalaryScheduleParser();
            ISalarySchedule schedule = parser.Process(serverFile);
            var viewModel = schedule.ToViewModel();
            return viewModel;
        }
    }
}
