using System;
using System.Collections.Generic;

namespace SalarySchedules.Models
{
    public class JobClass
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public BargainingUnit BargainingUnit { get; set; }
        public string Grade { get; set; }
        public IEnumerable<JobClassStep> Steps { get; set; }

        /// <summary>
        /// True if Title, Code, BargainingUnit, and BargainingUnit.Code
        /// are all non-null and non-empy. False otherwise.
        /// </summary>
        public bool WellDefined
        {
            get
            {
                return !(String.IsNullOrEmpty(Title)
                      || String.IsNullOrEmpty(Code)
                      || String.IsNullOrEmpty(Grade)
                      || BargainingUnit == null
                      || String.IsNullOrEmpty(BargainingUnit.Code));
            }
        }
    }
}
