using System.Web.Services;
using SalarySchedules.Models;
using SalarySchedules.Parser;
using System.Web;

namespace SalarySchedules.Web
{
    [WebService(Namespace = "http://wwww.smgov.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class SalaryScheduleService : System.Web.Services.WebService
    {
        [WebMethod]
        public SalaryScheduleDTO GetSchedule(string file)
        {
            var serverFile = HttpContext.Current.Server.MapPath(string.Format("~/Resources/{0}", file));
            var parser = new ScheduleParser();
            var schedule = parser.Process(serverFile);
            var dto = ((ISalarySchedule)schedule).ToDTO();
            return dto;
        }
    }
}
