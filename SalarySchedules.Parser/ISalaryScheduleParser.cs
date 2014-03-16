using SalarySchedules.Models;

namespace SalarySchedules.Parser
{
    public interface ISalaryScheduleParser
    {
        /// <summary>
        /// Create an ISalarySchedule object graph from a salary schedule file.
        /// </summary>
        /// <param name="filePath">Full file path to a (readable) salary schedule file.</param>
        /// <returns>The ISalarySchedule object representation of the given file.</returns>
        ISalarySchedule Process(string filePath);
    }
}
