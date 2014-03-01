using System;
using System.Collections.Generic;

namespace SalarySchedule.Models
{
    public class SalarySchedule
    {
        public DateTime ReportRunDate { get; set; }
        public TimeSpan FiscalYear { get; set; }
        public IEnumerable<JobClass> JobClasses { get; set; }
    }
}
