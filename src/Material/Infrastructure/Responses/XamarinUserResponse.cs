using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinShards
    {
        [DataMember(Name = "total")]
        public int? Total { get; set; }

        [DataMember(Name = "successful")]
        public int? Successful { get; set; }

        [DataMember(Name = "failed")]
        public int? Failed { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinIpInfo
    {
        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinUserTraits
    {

        [DataMember(Name = "email")]
        public string Email { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinDevice
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "os")]
        public string Os { get; set; }

        [DataMember(Name = "osVersion")]
        public string OsVersion { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "model")]
        public string Model { get; set; }

        [DataMember(Name = "appVersion")]
        public string AppVersion { get; set; }

        [DataMember(Name = "lastSeenTimestamp")]
        public int? LastSeenTimestamp { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinUser
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "ipInfo")]
        public XamarinIpInfo IpInfo { get; set; }

        [DataMember(Name = "traits")]
        public XamarinUserTraits Traits { get; set; }

        [DataMember(Name = "createdTimestamp")]
        public int? CreatedTimestamp { get; set; }

        [DataMember(Name = "deviceTimestamp")]
        public int? DeviceTimestamp { get; set; }

        [DataMember(Name = "systemTimestamp")]
        public int? SystemTimestamp { get; set; }

        [DataMember(Name = "lastSeenTimestamp")]
        public int? LastSeenTimestamp { get; set; }

        [DataMember(Name = "type")]
        public int? Type { get; set; }

        [DataMember(Name = "sessionsCount")]
        public int? SessionsCount { get; set; }

        [DataMember(Name = "devices")]
        public IList<XamarinDevice> Devices { get; set; }

        [DataMember(Name = "appId")]
        public string AppId { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinUserHit
    {
        [DataMember(Name = "_index")]
        public string Index { get; set; }

        [DataMember(Name = "_type")]
        public string Type { get; set; }

        [DataMember(Name = "_id")]
        public string Id { get; set; }

        [DataMember(Name = "_score")]
        public int? Score { get; set; }

        [DataMember(Name = "_source")]
        public XamarinUser Source { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinUserHits
    {
        [DataMember(Name = "total")]
        public int? Total { get; set; }

        [DataMember(Name = "max_score")]
        public int? MaxScore { get; set; }

        [DataMember(Name = "hits")]
        public IList<XamarinUserHit> HitList { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinUserResponse
    {
        [DataMember(Name = "took")]
        public int? Took { get; set; }

        [DataMember(Name = "timed_out")]
        public bool TimedOut { get; set; }

        [DataMember(Name = "_shards")]
        public XamarinShards Shards { get; set; }

        [DataMember(Name = "hits")]
        public XamarinUserHits Hits { get; set; }
    }
}
