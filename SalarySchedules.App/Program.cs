using SalarySchedules.Models;
using SalarySchedules.Parser;
using System.IO;
using System.Web.Script.Serialization;

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

            ScheduleParser parser = new ScheduleParser();
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
