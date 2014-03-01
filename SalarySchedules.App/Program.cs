using SalarySchedules.Models;
using SalarySchedules.Parser;
using System.Collections.Generic;

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

            List<SalarySchedule> shedules = new List<SalarySchedule>();
            ScheduleParser parser = new ScheduleParser();

            foreach (var file in args)
            {
                SalarySchedule schedule = parser.Process(file);
                shedules.Add(schedule);
            }
        }
    }
}
