using System.Linq;
using SalarySchedules.Models;
using System;

namespace SalarySchedules.Web
{
    public static class Extensions
    {
        private static readonly string moneyFormat = "{0:C}";

        public static SalarySchedule ToViewModel(this ISalarySchedule schedule)
        {
            return new SalarySchedule()
            {
                ReportRunDate = schedule.ReportRunDate,
                FiscalYear = schedule.FiscalYear,
                BargainingUnits = schedule.BargainingUnits.ToArray(),
                JobClasses = schedule.JobClasses.Select(jc => jc.ToViewModel()).ToArray()
            };
        }

        public static JobClass ToViewModel(this IJobClass jobClass)
        {
            return new JobClass()
            {
                BargainingUnit = jobClass.BargainingUnit,
                Code = jobClass.Code,
                Grade = jobClass.Grade,
                Steps = jobClass.Steps.Select(s => s.ToViewModel()).ToArray(),
                Title = jobClass.Title
            };
        }

        public static JobClassStep ToViewModel(this Models.JobClassStep jobClassStep)
        {
            return new JobClassStep()
            {
                StepNumber = jobClassStep.StepNumber,
                HourlyRate = String.Format(moneyFormat, jobClassStep.HourlyRate.HasValue ? jobClassStep.HourlyRate.Value : 0),
                BiWeeklyRate = String.Format(moneyFormat, jobClassStep.BiWeeklyRate.HasValue ? jobClassStep.BiWeeklyRate.Value : 0),
                MonthlyRate = String.Format(moneyFormat, jobClassStep.MonthlyRate.HasValue ? jobClassStep.MonthlyRate.Value : 0),
                AnnualRate = String.Format(moneyFormat, jobClassStep.AnnualRate.HasValue ? jobClassStep.AnnualRate.Value : 0)
            };
        }
    }
}