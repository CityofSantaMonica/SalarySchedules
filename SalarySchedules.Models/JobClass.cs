using System.Collections.Generic;

namespace SalarySchedules.Models
{
    public class JobClass
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string BargainingUnit { get; set; }
        public string Grade { get; set; }
        public IEnumerable<JobClassStep> Steps { get; set; }
    }
}
