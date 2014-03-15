using SalarySchedules.Models;

namespace SalarySchedules.Parser
{
    public interface ISalaryScheduleParser
    {
        /// <summary>
        /// Create a SalarySchedule object graph from a salary schedule file.
        /// </summary>
        /// <param name="filePath">Full file path to a (readable) salary schedule file.</param>
        /// <returns>The SalarySchedule object representation of the given file.</returns>
        public SalarySchedule Process(string filePath);
    }
}
