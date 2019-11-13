using System;

namespace RocketLunch.domain.utilities
{
    public static class DateTimeExtensions
    {
        public static int GetUnixTime(this DateTime time)
        {
            return (Int32)(time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}