using System;

namespace SalarySchedules.Models
{
    public class FiscalYear
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// e.g. 11/12, 12/13, 13/14
        /// </summary>
        public string ShortSpanCode
        {
            get
            {
                if (StartDate.HasValue && EndDate.HasValue)
                    return String.Format("{0}/{1}", StartDate.Value.ToString("yy"), EndDate.Value.ToString("yy"));
                else
                    return String.Empty;
            }
        }
    }
}
