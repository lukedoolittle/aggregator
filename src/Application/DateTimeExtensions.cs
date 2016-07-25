using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Aggregator.Framework.Extensions
{
    //TODO: remove this after we do the FilterableRequest and TimeseriesRequest
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts a string into a datetime offset struct. If the string does not contain offset
        /// information that information should be provided through the offset parameter
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="format"></param>
        /// <param name="offset">Seperate offset string</param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(
            this string instance, 
            string format, 
            string offset)
        {
            if (format == null)
            {
                long result;

                if (long.TryParse(instance, out result))
                {
                    DateTimeOffset timestamp = result.ToDateTime();
                    return timestamp.ToOffset(offset.ToTimespan());
                }
            }
            else
            {
                DateTimeOffset result;

                if (offset == null)
                {
                    if (DateTimeOffset.TryParseExact(
                            instance,
                            format,
                            null,
                            DateTimeStyles.AssumeLocal,
                            out result))
                    {
                        return result;
                    }
                }
                else
                {
                    DateTime dateTime;
                    if (DateTime.TryParseExact(
                        instance, 
                        format, 
                        null, 
                        DateTimeStyles.None, 
                        out dateTime))
                    {
                        return new DateTimeOffset(
                            dateTime, 
                            offset.ToTimespan());
                    }
                }
            }

            throw new FormatException();
        }

        private static int _nextYear => DateTime.Now.Year + 1;

        /// <summary>
        /// Converts a string into a timespan representing the datetime offset in hours, minutes or seconds
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        private static TimeSpan ToTimespan(this string instance)
        {
            if (instance == null)
            {
                return new TimeSpan(0, 0, 0);
            }

            var count = Convert.ToInt32(instance);

            if ((count >= 0 && count <= 12) ||
                (count <= 0 && count >= -12))
            {
                return new TimeSpan(count, 0, 0);
            }

            if ((count <= 720 && count >= 0) ||
                (count >= -720 && count < 0))
            {
                return new TimeSpan(count / 60, 0, 0);
            }

            return new TimeSpan(count / 3600000, 0, 0);
        }


        /// <summary>
        /// Converts a long representing either seconds or milliseconds since epoch into a datetime struct
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long instance)
        {
            var datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            try
            {
                datetime = datetime.AddSeconds(instance);
                if (datetime.Year < _nextYear)
                {
                    return datetime;
                }
                else
                {
                    datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    return datetime.AddMilliseconds(instance);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return datetime.AddMilliseconds(instance);
            }
        }
    }
}
