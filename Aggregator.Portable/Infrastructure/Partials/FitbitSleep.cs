using System;
using System.Collections.Generic;


namespace Aggregator.Infrastructure.Requests
{
    public partial class FitbitSleep
    {
        public static string Date => "date";
        public static string DateTime => "dateTime";
        public static string DateFormat => "yyyy-MM-dd";
        public static int BackdatedDays => 7;
    }
}
