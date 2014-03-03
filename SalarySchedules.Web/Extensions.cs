using System.Linq;
using SalarySchedules.Models;

namespace SalarySchedules.Web
{
    public static class Extensions
    {
        public static SalaryScheduleDTO ToDTO(this ISalarySchedule schedule)
        {
            return new SalaryScheduleDTO()
            {
                ReportRunDate = schedule.ReportRunDate,
                FiscalYear = schedule.FiscalYear,
                BargainingUnits = schedule.BargainingUnits.ToArray(),
                JobClasses = schedule.JobClasses.Select(jc => jc.ToDTO()).ToArray()
            };
        }

        public static JobClassDTO ToDTO(this IJobClass jobClass)
        {
            return new JobClassDTO()
            {
                BargainingUnit = jobClass.BargainingUnit,
                Code = jobClass.Code,
                Grade = jobClass.Grade,
                Steps = jobClass.Steps.ToArray(),
                Title = jobClass.Title
            };
        }
    }
}