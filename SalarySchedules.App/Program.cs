using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SalarySchedules.Models;
using SalarySchedules.Parser;

namespace SalarySchedules.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = args.Where(a => a.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase));

            if (!files.Any())
            {
                Console.WriteLine("USAGE:");
                Console.WriteLine("\tSalarySchedules file1.pdf [file2.pdf file3.pdf fileN.pdf]");
                return;
            }

            ISalaryScheduleParser parser = new CSMSalaryScheduleParser();

            foreach (var file in files)
            {
                try
                {
                    Console.Write("Processing file {0}... ", file);
                    ISalarySchedule schedule = parser.Process(file);
                    string json = JsonConvert.SerializeObject(schedule);
                    File.WriteAllText(file.Replace(".pdf", ".json"), json);
                    Console.WriteLine("Finished");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error processing file {0}", file);
                    Console.Write(ex);
                }
            }
        }
    }
}
