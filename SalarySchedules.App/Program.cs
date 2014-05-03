using System.IO;
using System.Web.Script.Serialization;
using SalarySchedules.Models;
using SalarySchedules.Parser;
using System;

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
                try
                {
                    ISalarySchedule schedule = parser.Process(file);
                    string json = serializer.Serialize(schedule);
                    File.WriteAllText(file.Replace(".pdf", ".json"), json);
                }
                catch
                {
                    Console.WriteLine("Error processing file {0}", file);
                }
            }
        }
    }
}
