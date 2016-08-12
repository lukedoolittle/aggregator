using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Foundations.HttpClient.Metadata;

namespace Material.Infrastructure.Responses
{
    [DatetimeFormatter("yyyy-MM-ddTHH:mm:ssZ")]
    [DataContract]
    public class SpotifySavedTrackResponse
    {
        [DataMember(Name = "href")]
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [DataMember(Name = "items")]
        [JsonProperty(PropertyName = "items")]
        public IList<SpotifyTrackItem> Items { get; set; }
        [DataMember(Name = "limit")]
        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }
        [DataMember(Name = "next")]
        [JsonProperty(PropertyName = "next")]
        public object Next { get; set; }
        [DataMember(Name = "offset")]
        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }
        [DataMember(Name = "previous")]
        [JsonProperty(PropertyName = "previous")]
        public object Previous { get; set; }
        [DataMember(Name = "total")]
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }

    [DataContract]
    public class SpotifyTrack
    {
        [DataMember(Name = "album")]
        [JsonProperty(PropertyName = "album")]
        public SpotifyAlbum Album { get; set; }
        [DataMember(Name = "artists")]
        [JsonProperty(PropertyName = "artists")]
        public IList<SpotifyArtist> Artists { get; set; }
        [DataMember(Name = "available_markets")]
        [JsonProperty(PropertyName = "available_markets")]
        public IList<string> AvailableMarkets { get; set; }
        [DataMember(Name = "disc_number")]
        [JsonProperty(PropertyName = "disc_number")]
        public int DiscNumber { get; set; }
        [DataMember(Name = "duration_ms")]
        [JsonProperty(PropertyName = "duration_ms")]
        public int DurationMs { get; set; }
        public bool @explicit { get; set; }
        public Dictionary<string, string> external_ids { get; set; }
        public Dictionary<string, string> external_urls { get; set; }
        [DataMember(Name = "href")]
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "popularity")]
        [JsonProperty(PropertyName = "popularity")]
        public int Popularity { get; set; }
        [DataMember(Name = "preview_url")]
        [JsonProperty(PropertyName = "preview_url")]
        public string PreviewUrl { get; set; }
        [DataMember(Name = "track_number")]
        [JsonProperty(PropertyName = "track_number")]
        public int TrackNumber { get; set; }
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "uri")]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }

    [DataContract]
    public class SpotifyTrackItem
    {
        [DataMember(Name = "added_at")]
        [JsonProperty(PropertyName = "added_at")]
        public DateTime AddedAt { get; set; }
        [DataMember(Name = "track")]
        [JsonProperty(PropertyName = "track")]
        public SpotifyTrack Track { get; set; }
    }

    [DataContract]
    public class SpotifyImage
    {
        [DataMember(Name = "height")]
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }
        [DataMember(Name = "url")]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [DataMember(Name = "width")]
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }
    }

    [DataContract]
    public class SpotifyAlbum
    {
        [DataMember(Name = "album_type")]
        [JsonProperty(PropertyName = "album_type")]
        public string AlbumType { get; set; }
        [DataMember(Name = "available_markets")]
        [JsonProperty(PropertyName = "available_markets")]
        public IList<string> AvailableMarkets { get; set; }
        public Dictionary<string, string> external_urls { get; set; }
        [DataMember(Name = "href")]
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "images")]
        [JsonProperty(PropertyName = "images")]
        public IList<SpotifyImage> Images { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "uri")]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }

    [DataContract]
    public class SpotifyArtist
    {
        public Dictionary<string, string> external_urls { get; set; }
        [DataMember(Name = "href")]
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "uri")]
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}
