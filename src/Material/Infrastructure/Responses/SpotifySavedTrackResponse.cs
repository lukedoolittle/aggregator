using System.Runtime.Serialization;
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
        public string Href { get; set; }
        [DataMember(Name = "items")]
        public IList<SpotifyTrackItem> Items { get; set; }
        [DataMember(Name = "limit")]
        public int Limit { get; set; }
        [DataMember(Name = "next")]
        public object Next { get; set; }
        [DataMember(Name = "offset")]
        public int Offset { get; set; }
        [DataMember(Name = "previous")]
        public object Previous { get; set; }
        [DataMember(Name = "total")]
        public int Total { get; set; }
    }

    [DataContract]
    public class SpotifyTrack
    {
        [DataMember(Name = "album")]
        public SpotifyAlbum Album { get; set; }
        [DataMember(Name = "artists")]
        public IList<SpotifyArtist> Artists { get; set; }
        [DataMember(Name = "available_markets")]
        public IList<string> AvailableMarkets { get; set; }
        [DataMember(Name = "disc_number")]
        public int DiscNumber { get; set; }
        [DataMember(Name = "duration_ms")]
        public int DurationMs { get; set; }
        public bool @explicit { get; set; }
        public Dictionary<string, string> external_ids { get; set; }
        public Dictionary<string, string> external_urls { get; set; }
        [DataMember(Name = "href")]
        public string Href { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "popularity")]
        public int Popularity { get; set; }
        [DataMember(Name = "preview_url")]
        public string PreviewUrl { get; set; }
        [DataMember(Name = "track_number")]
        public int TrackNumber { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "uri")]
        public string Uri { get; set; }
    }

    [DataContract]
    public class SpotifyTrackItem
    {
        [DataMember(Name = "added_at")]
        public DateTime AddedAt { get; set; }
        [DataMember(Name = "track")]
        public SpotifyTrack Track { get; set; }
    }

    [DataContract]
    public class SpotifyImage
    {
        [DataMember(Name = "height")]
        public int Height { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "width")]
        public int Width { get; set; }
    }

    [DataContract]
    public class SpotifyAlbum
    {
        [DataMember(Name = "album_type")]
        public string AlbumType { get; set; }
        [DataMember(Name = "available_markets")]
        public IList<string> AvailableMarkets { get; set; }
        public Dictionary<string, string> external_urls { get; set; }
        [DataMember(Name = "href")]
        public string Href { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "images")]
        public IList<SpotifyImage> Images { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "uri")]
        public string Uri { get; set; }
    }

    [DataContract]
    public class SpotifyArtist
    {
        public Dictionary<string, string> external_urls { get; set; }
        [DataMember(Name = "href")]
        public string Href { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "uri")]
        public string Uri { get; set; }
    }
}
