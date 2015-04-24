using System;
using System.IO;
using Newtonsoft.Json;
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

            foreach (var file in args)
            {
                try
                {
                    ISalarySchedule schedule = parser.Process(file);
                    string json = JsonConvert.SerializeObject(schedule);
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
