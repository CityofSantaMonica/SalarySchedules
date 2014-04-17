using System;
using System.Collections.Generic;

namespace SalarySchedules.Web
{
    public class JobClass : Models.IJobClass
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Grade { get; set; }
        public Models.BargainingUnit BargainingUnit { get; set; }
        public JobClassStep[] Steps { get; set; }

        IEnumerable<Models.JobClassStep> Models.IJobClass.Steps
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
