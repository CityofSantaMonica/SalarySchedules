using System;

namespace SalarySchedules.Models
{
    public static class Extensions
    {
        public static bool HasNoValue<T>(this Nullable<T> nullable) where T : struct
        {
            return !nullable.HasValue;
        }
    }
}
