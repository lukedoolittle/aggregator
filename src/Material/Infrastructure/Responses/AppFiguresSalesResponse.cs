using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Foundations.HttpClient.Metadata;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DateTimeFormatter("yyyy-MM-ddTHH:mm:ss")]
    public class AppFiguresSalesResponse : Dictionary<string, AppFiguresSalesEntry>
    {
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class AppFiguresSalesSource
    {
        [DataMember(Name = "external_account_id")]
        public string ExternalAccountId { get; set; }

        [DataMember(Name = "added_timestamp")]
        public DateTime AddedTimestamp { get; set; }

        [DataMember(Name = "active")]
        public bool Active { get; set; }

        [DataMember(Name = "hidden")]
        public bool Hidden { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class AppFiguresSalesEntry
    {
        [DataMember(Name = "downloads")]
        public int Downloads { get; set; }

        [DataMember(Name = "re_downloads")]
        public int ReDownloads { get; set; }

        [DataMember(Name = "updates")]
        public int Updates { get; set; }

        [DataMember(Name = "returns")]
        public int Returns { get; set; }

        [DataMember(Name = "net_downloads")]
        public int NetDownloads { get; set; }

        [DataMember(Name = "promos")]
        public int Promos { get; set; }

        [DataMember(Name = "revenue")]
        public string Revenue { get; set; }

        [DataMember(Name = "returns_amount")]
        public string ReturnsAmount { get; set; }

        [DataMember(Name = "edu_downloads")]
        public int EduDownloads { get; set; }

        [DataMember(Name = "gifts")]
        public int Gifts { get; set; }

        [DataMember(Name = "gift_redemptions")]
        public int GiftRedemptions { get; set; }

        [DataMember(Name = "product")]
        public AppFiguresSalesProduct Product { get; set; }

        [DataMember(Name = "product_id")]
        public string ProductId { get; set; }

        [DataMember(Name = "date")]
        public string Date { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class AppFiguresSalesProduct
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "developer")]
        public string Developer { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "vendor_identifier")]
        public string VendorIdentifier { get; set; }

        [DataMember(Name = "ref_no")]
        public string RefNo { get; set; }

        [DataMember(Name = "sku")]
        public string Sku { get; set; }

        [DataMember(Name = "package_name")]
        public string PackageName { get; set; }

        [DataMember(Name = "store_id")]
        public int StoreId { get; set; }

        [DataMember(Name = "store")]
        public string Store { get; set; }

        [DataMember(Name = "storefront")]
        public string Storefront { get; set; }

        [DataMember(Name = "release_date")]
        public DateTime ReleaseDate { get; set; }

        [DataMember(Name = "added_date")]
        public DateTime AddedDate { get; set; }

        [DataMember(Name = "updated_date")]
        public DateTime UpdatedDate { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "active")]
        public bool Active { get; set; }

        [DataMember(Name = "source")]
        public AppFiguresSalesSource Source { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "devices")]
        public IList<string> Devices { get; set; }

        [DataMember(Name = "bundle_identifier")]
        public string BundleIdentifier { get; set; }

        [DataMember(Name = "accessible_features")]
        public IList<string> AccessibleFeatures { get; set; }

        [DataMember(Name = "children")]
        public IList<string> Children { get; set; }

        [DataMember(Name = "features")]
        public IList<string> Features { get; set; }

        [DataMember(Name = "parent_id")]
        public string ParentId { get; set; }

        [DataMember(Name = "storefronts")]
        public IList<string> Storefronts { get; set; }
    }
}
