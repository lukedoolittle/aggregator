﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Material.Framework.Metadata;
using Material.Domain.ResourceProviders;
using System;
using System.Collections.Generic;
using Material.Framework.Enums;
using System.Net;
using Material.Framework.Metadata.Formatters;
using Material.Domain.Requests;
using Material.Domain.Core;
using System.CodeDom.Compiler;

namespace Material.Domain.Requests
{     
    /// <summary>
    /// Get Performance KPIs
    /// </summary>
    [ServiceType(typeof(Rigor))]
	[GeneratedCode("T4Toolbox", "14.0")]
	public partial class RigorGetRealBrowserPerformanceKPIs : OAuthRequest              
	{
        public override String Host => "https://monitoring-api.rigor.com";
        public override String Path => "/v2/checks/real_browsers/{id}/pages/data";
        public override String HttpMethod => "GET";
        public override List<MediaType> Produces => new List<MediaType> { MediaType.Json };
        public override List<MediaType> Consumes => new List<MediaType> { MediaType.Json };
        public override List<HttpStatusCode> ExpectedStatusCodes => new List<HttpStatusCode> { HttpStatusCode.OK };
        /// <summary>
        /// The ID of the check to fetch.
        /// </summary>
        [Name("id")]
        [ParameterType(RequestParameterType.Path)]
        [Required()]
        [DefaultFormatter()]
        public  Nullable<Int32> Id { get; set; }
        /// <summary>
        /// The start time for the timeframe.
        /// </summary>
        [Name("from")]
        [ParameterType(RequestParameterType.Query)]
        [DateTimeFormatter("yyyy-MM-ddTHH:mm:sszzz")]
        public  Nullable<DateTime> From { get; set; }
        /// <summary>
        /// The end time for the timeframe.
        /// </summary>
        [Name("to")]
        [ParameterType(RequestParameterType.Query)]
        [DateTimeFormatter("yyyy-MM-ddTHH:mm:sszzz")]
        public  Nullable<DateTime> To { get; set; }
        /// <summary>
        /// A predefined timeframe to be used instead of `from` and `to`. Defaults to the last hour.
        /// </summary>
        [Name("range")]
        [ParameterType(RequestParameterType.Query)]
        [EnumFormatter()]
        public  RigorGetRealBrowserPerformanceKPIsRange Range { get; set; } = RigorGetRealBrowserPerformanceKPIsRange.LastHour;
        /// <summary>
        /// The metric names to get data for. Valid metrics include: `server_time`, `start_render`, `dom_load_time`, `onload_time`, `visually_complete`, `fully_loaded_time`, `speed_index`, `request_count`, `content_size`, `html_count`, `html_size`, `image_count`, `image_size`, `javascript_count`, `javascript_size`, `css_count`, `css_size`, `video_count`, `video_size`, `font_count`, `font_size`, `other_count`, `other_size`, `client_error_count`, `connection_error_count`, `server_error_count`, and `error_count`.                 Prefix the metric with `median_`, `average_`, `max_`, or `min_` when querying for                  more than 24 hours of data. For `count` metrics, `total_` is also available.
        /// </summary>
        [Name("metrics")]
        [ParameterType(RequestParameterType.Query)]
        [Required()]
        [EnumFormatter()]
        public  RigorGetRealBrowserPerformanceKPIsMetrics Metrics { get; set; }
        /// <summary>
        /// If true, include a series that sums results from all pages.                Useful to represent each run as a whole rather than each page individually.
        /// </summary>
        [Name("include_total")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> IncludeTotal { get; set; } = true;
        /// <summary>
        /// If true, include data from failed runs.
        /// </summary>
        [Name("include_failures")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> IncludeFailures { get; set; } = true;
        /// <summary>
        /// A list of location IDs used to filter the results.
        /// </summary>
        [Name("locations")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  String Locations { get; set; }
        /// <summary>
        /// A list of page IDs used to filter the results.
        /// </summary>
        [Name("page_ids")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  String PageIds { get; set; }
	}
	
	[GeneratedCode("T4Toolbox", "14.0")]
    public enum RigorGetRealBrowserPerformanceKPIsRange
    {
        [Description("last_hour")] LastHour,
        [Description("last_4_hours")] Last4Hours,
        [Description("last_8_hours")] Last8Hours,
        [Description("last_12_hours")] Last12Hours,
        [Description("last_24_hours")] Last24Hours,
        [Description("yesterday")] Yesterday,
        [Description("today")] Today,
        [Description("last_7_days")] Last7Days,
        [Description("last_30_days")] Last30Days,
        [Description("this_week")] ThisWeek,
        [Description("last_week")] LastWeek,
        [Description("this_month")] ThisMonth,
        [Description("last_month")] LastMonth,
        [Description("last_3_months")] Last3Months,
        [Description("last_6_months")] Last6Months,
        [Description("last_12_months")] Last12Months,
    }
	
	[GeneratedCode("T4Toolbox", "14.0")]
    public enum RigorGetRealBrowserPerformanceKPIsMetrics
    {
        [Description("server_time")] ServerTime,
        [Description("start_render")] StartRender,
        [Description("dom_load_time")] DomLoadTime,
        [Description("onload_time")] OnloadTime,
        [Description("visually_complete")] VisuallyComplete,
        [Description("fully_loaded_time")] FullyLoadedTime,
        [Description("speed_index")] SpeedIndex,
        [Description("request_count")] RequestCount,
        [Description("content_size")] ContentSize,
        [Description("html_count")] HtmlCount,
        [Description("html_size")] HtmlSize,
        [Description("image_count")] ImageCount,
        [Description("image_size")] ImageSize,
        [Description("javascript_count")] JavascriptCount,
        [Description("javascript_size")] JavascriptSize,
        [Description("css_count")] CssCount,
        [Description("css_size")] CssSize,
        [Description("video_count")] VideoCount,
        [Description("video_size")] VideoSize,
        [Description("font_count")] FontCount,
        [Description("font_size")] FontSize,
        [Description("other_count")] OtherCount,
        [Description("other_size")] OtherSize,
        [Description("client_error_count")] ClientErrorCount,
        [Description("connection_error_count")] ConnectionErrorCount,
        [Description("server_error_count")] ServerErrorCount,
        [Description("error_count")] ErrorCount,
    }
}
