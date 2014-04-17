using System;
using System.Collections.Generic;
using System.Linq;

namespace SalarySchedules.Web
{
    public class SalarySchedule : Models.ISalarySchedule
    {
        public DateTime? ReportRunDate { get; set; }
        public Models.FiscalYear FiscalYear { get; set; }
        public Models.BargainingUnit[] BargainingUnits { get; set; }
        public JobClass[] JobClasses { get; set; }

        IEnumerable<Models.BargainingUnit> Models.ISalarySchedule.BargainingUnits
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
        IEnumerable<Models.IJobClass> Models.ISalarySchedule.JobClasses
        {
            get
            {
                return JobClasses;
            }
            set
            {
                JobClasses = value.Select(jc => jc.ToViewModel()).ToArray();
            }
        }
    }
}
