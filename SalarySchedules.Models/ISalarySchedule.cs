using System;
using System.Collections.Generic;

namespace SalarySchedules.Models
{
    public interface ISalarySchedule
    {
        DateTime? ReportRunDate { get; set; }
        FiscalYear FiscalYear { get; set; }
        IEnumerable<BargainingUnit> BargainingUnits { get; set; }
        IEnumerable<IJobClass> JobClasses { get; set; }
    }
}
