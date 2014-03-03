using System;
using System.Collections.Generic;
using System.Linq;
using SalarySchedules.Models;

namespace SalarySchedules.Web
{
    public class JobClassDTO : IJobClass
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Grade { get; set; }
        public BargainingUnit BargainingUnit { get; set; }
        public JobClassStep[] Steps { get; set; }

        IEnumerable<JobClassStep> IJobClass.Steps
        {
            get
            {
                return Steps;
            }
            set
            {
                Steps = value.ToArray();
            }
        }
    }
}
