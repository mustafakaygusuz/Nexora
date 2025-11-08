namespace Nexora.Core.Common.Extensions
{
    public static class DateExtension
    {
        public static DateTime GetEndOfWeek(this DateTime date)
        {
            var daysToSunday = ((int)DayOfWeek.Sunday - (int)date.DayOfWeek + 7) % 7;
            return date.AddDays(daysToSunday).Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime GetEndOfMonth(this DateTime date)
        {
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return new DateTime(date.Year, date.Month, daysInMonth).Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime GetEndOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31).Date.AddDays(1).AddTicks(-1);
        }
    }
}