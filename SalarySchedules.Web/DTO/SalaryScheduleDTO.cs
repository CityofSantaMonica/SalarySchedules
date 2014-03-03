using System;
using System.Collections.Generic;
using System.Linq;
using SalarySchedules.Models;

namespace SalarySchedules.Web
{
    public class SalaryScheduleDTO : ISalarySchedule
    {
        public DateTime? ReportRunDate { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public BargainingUnit[] BargainingUnits { get; set; }
        public JobClassDTO[] JobClasses { get; set; }

        IEnumerable<BargainingUnit> ISalarySchedule.BargainingUnits
        {
            get
            {
                return BargainingUnits;
            }
            set
            {
                BargainingUnits = value.ToArray();
            }
        }
        IEnumerable<IJobClass> ISalarySchedule.JobClasses
        {
            get
            {
                return JobClasses;
            }
            set
            {
                JobClasses = value.Select(jc => jc.ToDTO()).ToArray();
            }
        }
    }
}
