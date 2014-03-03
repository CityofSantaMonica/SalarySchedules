using System.Text.RegularExpressions;

namespace SalarySchedules.Parser
{
    static class FieldPatterns
    {
        public static Regex FiscalYear =
            new Regex(@"fiscal\s+year\s+(\d{2})/(\d{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex RunDate = 
            new Regex(@"run date: (\d{1,2}/\d{1,2}/\d{2,4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex DataHeader = 
            new Regex(@"Class Title Class Code BU Grade Step Hourly Rate Monthly Rate Annual Rate Bi-Weekly Rate", RegexOptions.Compiled);

        public static Regex ClassTitle = 
            new Regex(@"[a-z\./&-]+ ([a-z\./&\(\) -])+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex ClassCode =
            new Regex(@"(^(?<value>[0-9]{4})|\s+(?<value>[0-9]{4}))\s?", RegexOptions.Compiled);

        public static Regex BargainingUnit =
            new Regex(@"\s?(?<value>[A-Z]{3})\s?", RegexOptions.Compiled);

        public static Regex Grade =
            new Regex(@"(^(?<value>[0-9]{3})|\s+(?<value>[0-9]{3}))\s?", RegexOptions.Compiled);

        public static Regex Step =
            new Regex(@"\s?(?<value>[1-5])\s?", RegexOptions.Compiled);

        public static Regex Rate =
            new Regex(@"\s?(?<value>[0-9,]{1,}\.[0-9]{2})\s?", RegexOptions.Compiled);
    }
}
