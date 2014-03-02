namespace SalarySchedules.Models
{
    public class JobClassStep
    {
        public int? StepNumber { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? MonthlyRate { get; set; }
        public decimal? AnnualRate { get; set; }
        public decimal? BiWeeklyRate { get; set; }

        /// <summary>
        /// True if StepNumber, HourlyRate, MonthlyRate, AnnualRate, and BiWeeklyRate
        /// have all been given values. False otherwise.
        /// </summary>
        public bool WellDefined
        {
            get
            {
                return StepNumber.HasValue
                        && HourlyRate.HasValue
                         && MonthlyRate.HasValue
                          && AnnualRate.HasValue
                           && BiWeeklyRate.HasValue;
            }
        }
    }
}
