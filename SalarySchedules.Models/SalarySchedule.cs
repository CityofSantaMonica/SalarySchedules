using System;
using System.Collections.Generic;

namespace SalarySchedules.Models
{
    public class SalarySchedule : ISalarySchedule
    {
        public DateTime? ReportRunDate { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public IEnumerable<BargainingUnit> BargainingUnits { get; set; }
        public IEnumerable<IJobClass> JobClasses { get; set; }
    }
}
