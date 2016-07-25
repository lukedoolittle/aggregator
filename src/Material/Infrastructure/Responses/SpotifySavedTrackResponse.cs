using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class SpotifySavedTrackResponse
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [JsonProperty(PropertyName = "items")]
        public IList<SpotifyTrackItem> Items { get; set; }
        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }
        [JsonProperty(PropertyName = "next")]
        public object Next { get; set; }
        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }
        [JsonProperty(PropertyName = "previous")]
        public object Previous { get; set; }
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }

    public class SpotifyTrack
    {
        [JsonProperty(PropertyName = "album")]
        public SpotifyAlbum Album { get; set; }
        [JsonProperty(PropertyName = "artists")]
        public IList<SpotifyArtist> Artists { get; set; }
        [JsonProperty(PropertyName = "available_markets")]
        public IList<string> AvailableMarkets { get; set; }
        [JsonProperty(PropertyName = "disc_number")]
        public int DiscNumber { get; set; }
        [JsonProperty(PropertyName = "duration_ms")]
        public int DurationMs { get; set; }
        public bool @explicit { get; set; }
        public Dictionary<string, string> external_ids { get; set; }
        public Dictionary<string, string> external_urls { get; set; }
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "popularity")]
        public int Popularity { get; set; }
        [JsonProperty(PropertyName = "preview_url")]
        public string PreviewUrl { get; set; }
        [JsonProperty(PropertyName = "track_number")]
        public int TrackNumber { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }

    public class SpotifyTrackItem
    {
        [JsonProperty(PropertyName = "added_at")]
        public DateTime AddedAt { get; set; }
        [JsonProperty(PropertyName = "track")]
        public SpotifyTrack Track { get; set; }
    }

    public class SpotifyImage
    {
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }
    }

    public class SpotifyAlbum
    {
        [JsonProperty(PropertyName = "album_type")]
        public string AlbumType { get; set; }
        [JsonProperty(PropertyName = "available_markets")]
        public IList<string> AvailableMarkets { get; set; }
        public Dictionary<string, string> external_urls { get; set; }
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "images")]
        public IList<SpotifyImage> Images { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }

    public class SpotifyArtist
    {
        public Dictionary<string, string> external_urls { get; set; }
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}
