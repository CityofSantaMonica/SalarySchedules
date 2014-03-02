using System.Text.RegularExpressions;

namespace SalarySchedules.Parser
{
    static class FieldPatterns
    {
        public static Regex FiscalYear = new Regex(@"fiscal\s+year\s+(\d{2})/(\d{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex RunDate = new Regex(@"run date: (\d{1,2}/\d{1,2}/\d{2,4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex DataHeader = new Regex(@"Class Title Class Code BU Grade Step Hourly Rate Monthly Rate Annual Rate Bi-Weekly Rate", RegexOptions.Compiled);
        public static Regex ClassTitle = new Regex(@"[a-z\./&-]+ ([a-z\./& -])+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex ClassCode = new Regex(@"[0-9]{4}", RegexOptions.Compiled);
        public static Regex BargainingUnit = new Regex(@"\s[A-Z]{3}", RegexOptions.Compiled);
        public static Regex Grade = new Regex(@"[0-9]{3}", RegexOptions.Compiled);
        public static Regex Step = new Regex(@"\s*[1-5]\s*", RegexOptions.Compiled);
        public static Regex Rate = new Regex(@"\s[0-9,]{1,}\.[0-9]{2}", RegexOptions.Compiled);
        public static Regex ConsecutiveSpaces = new Regex(@"\s{2,}", RegexOptions.Compiled);
    }
}
