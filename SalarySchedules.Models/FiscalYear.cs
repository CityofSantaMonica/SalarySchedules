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

        public FiscalYear() { }

        public FiscalYear(string startValue, string endValue)
        {
            startValue = startValue.Length == 2 ? String.Format("20{0}", startValue) : startValue;
            endValue = endValue.Length == 2 ? String.Format("20{0}", endValue) : endValue;

            StartDate = new DateTime(Int16.Parse(startValue), 7, 1, 0, 0, 0, DateTimeKind.Unspecified);
            EndDate = new DateTime(Int16.Parse(endValue), 6, 30, 11, 59, 59, DateTimeKind.Unspecified);
        }
    }
}
