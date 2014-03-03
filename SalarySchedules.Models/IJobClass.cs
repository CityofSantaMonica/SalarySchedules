using System.Collections.Generic;

namespace SalarySchedules.Models
{
    public interface IJobClass
    {
        string Title { get; set; }
        string Code { get; set; }
        string Grade { get; set; }
        BargainingUnit BargainingUnit { get; set; }
        IEnumerable<JobClassStep> Steps { get; set; }
    }
}
