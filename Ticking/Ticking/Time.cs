using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Ticking.Essentials
{
    public static class Time
    {
        public static TimeSpan Minute => TimeSpan.FromMinutes(1);

        public static TimeSpan Second => TimeSpan.FromSeconds(1);

        public static TimeSpan Day => TimeSpan.FromDays(1);

        public static TimeSpan Hour => TimeSpan.FromHours(1);

        public static DateTime FirstDayOfWeek(this DateTime dateTime, CultureInfo cultureInfo)
            => dateTime.AddDays(cultureInfo.DateTimeFormat.FirstDayOfWeek - dateTime.DayOfWeek);

        public static DateTime FirstDayOfWeek(this DateTime dateTime)
            => FirstDayOfWeek(dateTime, Thread.CurrentThread.CurrentCulture);

        public static DateTime LastDayOfWeek(this DateTime dateTime, CultureInfo cultureInfo)
            => FirstDayOfWeek(dateTime, cultureInfo).AddDays(6);

        public static DateTime LastDayOfWeek(this DateTime dateTime)
            => LastDayOfWeek(dateTime, Thread.CurrentThread.CurrentCulture);

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1) + dateTime.TimeOfDay;

        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            var days = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return new DateTime(dateTime.Year, dateTime.Month, days) + dateTime.TimeOfDay;
        }

        public static DateTime FirstDayOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1) + dateTime.TimeOfDay;
        }

        public static DateTime LastDayOfYear(this DateTime dateTime)
        {
            var days = DateTime.DaysInMonth(dateTime.Year, 12);
            return new DateTime(dateTime.Year, 12, days) + dateTime.TimeOfDay;
        }

        public static DateTime WithoutTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public static IEnumerable<DateTime> Days(this DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            for (var day = dateTimeFrom; day < dateTimeTo; day = day.AddDays(1))
                yield return day.WithoutTime();
        }

        public static IEnumerable<DateTime> DaysInMonth(this DateTime dateTime)
        {
            var from = dateTime.FirstDayOfMonth().WithoutTime();
            return from.Days(from.AddMonths(1));
        }

        public static DateTime NextDateOfDay(this DateTime dateTime, DayOfWeek day)
        {
            int days = day - dateTime.DayOfWeek <= 0
                ? day - dateTime.DayOfWeek + 7
                : day - dateTime.DayOfWeek;
            return dateTime.AddDays(days);
        }

        public static int Age(this DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var age = dateTimeTo.Year - dateTimeFrom.Year;
            return dateTimeTo < dateTimeFrom.AddYears(age)
                ? age - 1
                : age;
        }

        public static double ToUnixTimeStamp(this DateTime dateTime)
            => dateTime
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds;

        public static DateTime ParseDateTime(this string @string)
            => DateTime.Parse(@string);
    }
}
