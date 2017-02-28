using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Linq;
using Foundations.Attributes;
using Foundations.Extensions;

namespace Material.Infrastructure.Requests
{
    partial class YahooFlurryMetrics
    {
        private const char DIMENSION_SEPERATOR = '/';
        private const char METRIC_SEPERATOR = ',';
        private const string DATETIME_FORMAT = "yyyy-MM-dd";

        public YahooFlurryMetrics AddDimension(
            YahooFlurryMetricsDimension dimension)
        {
            if (Dimensions == null)
            {
                Dimensions = dimension.EnumToString();
            }
            else
            {
                var currentDimensions = Dimensions.Split(DIMENSION_SEPERATOR).ToList();
                currentDimensions.Add(dimension.EnumToString());
                Dimensions = string.Join(DIMENSION_SEPERATOR.ToString(), currentDimensions);
            }

            return this;
        }

        public YahooFlurryMetrics AddMetric(
            YahooFlurryMetricsMetrics metric)
        {
            if (Metrics == null)
            {
                Metrics = metric.EnumToString();
            }
            else
            {
                var currentMetrics = Metrics.Split(METRIC_SEPERATOR).ToList();
                currentMetrics.Add(metric.EnumToString());
                Metrics = string.Join(METRIC_SEPERATOR.ToString(), currentMetrics);
            }

            return this;
        }

        public YahooFlurryMetrics AddDateRange(
            DateTime startDate, 
            DateTime endDate)
        {
            DateTime = string.Format(
                CultureInfo.InvariantCulture, 
                "{0}/{1}", 
                startDate.ToString(DATETIME_FORMAT),
                endDate.ToString(DATETIME_FORMAT));

            return this;
        }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    public enum YahooFlurryMetricsDimension
    {
        [Description("company")] Company,
        [Description("app")] App,
        [Description("appVersion")] AppVersion,
        [Description("country")] Country,
        [Description("language")] Language,
        [Description("region")] Region,
        [Description("category")] Category,
        [Description("events")] Events
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    public enum YahooFlurryMetricsMetrics
    {
        [Description("sessions")] Sessions,
        [Description("activeDevices")] ActiveDevices,
        [Description("newDevices")] NewDevices,
        [Description("timeSpent")] TimeSpent,
        [Description("averageTimePerDevice")] AverageTimePerDevice,
        [Description("averageTimePerSession")] AverageTimePerSession,
        [Description("occurences")] Occurences
    }
}
