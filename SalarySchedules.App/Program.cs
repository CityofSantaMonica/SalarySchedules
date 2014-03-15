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

            ISalaryScheduleParser parser = new CSMSalaryScheduleParser();
            JavaScriptSerializer serializer = new JavaScriptSerializer(); 

            foreach (var file in args)
            {
                ISalarySchedule schedule = parser.Process(file);
                string json = serializer.Serialize(schedule);
                File.WriteAllText(file.Replace(".pdf", ".json"), json);
            }
        }
    }
}
