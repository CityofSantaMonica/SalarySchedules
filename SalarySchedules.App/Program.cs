using System.IO;
using System.Web.Script.Serialization;
using SalarySchedules.Models;
using SalarySchedules.Parser;

namespace SalarySchedules.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            CSMScheduleParser parser = new CSMScheduleParser();
            JavaScriptSerializer serializer = new JavaScriptSerializer(); 

            foreach (var file in args)
            {
                SalarySchedule schedule = parser.Process(file);
                string json = serializer.Serialize(schedule);
                File.WriteAllText(file.Replace(".pdf", ".json"), json);
            }
        }
    }
}
