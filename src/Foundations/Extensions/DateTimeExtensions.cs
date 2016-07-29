using System;

namespace Foundations.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts a DateTime into milliseconds since epoch
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static double ToUnixTimeMilliseconds(this DateTime instance)
        {
            var timespanSinceEpoch = instance - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return timespanSinceEpoch.TotalMilliseconds;
        }

        /// <summary>
        /// Converts a DateTime into seconds since epoch
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static double ToUnixTimeSeconds(this DateTime instance)
        {
            var timespanSinceEpoch = instance - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return timespanSinceEpoch.TotalSeconds;
        }

        /// <summary>
        /// Converts a DateTimeOffset into UTC seconds since epoch
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static double ToUnixTimeSeconds(this DateTimeOffset instance)
        {
            var timespanSinceEpoch = instance - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return timespanSinceEpoch.TotalSeconds;
        }

        /// <summary>
        /// Converts a DateTime into days since epoch
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static double ToUnixTimeDays(this DateTimeOffset instance)
        {
            var timespanSinceEpoch = instance - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return timespanSinceEpoch.TotalDays;
        }

        /// <summary>
        /// Converts a DateTimeOffset into UTC days since epoch
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static double ToUnixTimeDays(this DateTime instance)
        {
            var timespanSinceEpoch = instance - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return timespanSinceEpoch.Days;
        }

        public static DateTimeOffset FromUnixTimeMilliseconds(this long instance)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return new DateTimeOffset(epoch.AddMilliseconds(instance));
        }
    }
}
