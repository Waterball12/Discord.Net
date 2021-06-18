using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discord.Webhook.Common
{
    public class Embed
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("color")]
        public uint? Color { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTimeOffset? Timestamp { get; set; }
        [JsonPropertyName("author")]
        public EmbedAuthor? Author { get; set; }
        [JsonPropertyName("footer")]
        public EmbedFooter? Footer { get; set; }
        [JsonPropertyName("thumbnail")]
        public EmbedThumbnail? Thumbnail { get; set; }
        [JsonPropertyName("image")]
        public EmbedImage? Image { get; set; }
        [JsonPropertyName("provider")]
        public EmbedProvider? Provider { get; set; }
        [JsonPropertyName("fields")]
        public EmbedField[]? Fields { get; set; }
    }

    public  class EmbedFooter
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }
        [JsonPropertyName("proxy_icon_url")]
        public string ProxyIconUrl { get; set; }
    }

    public  class EmbedAuthor
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }
        [JsonPropertyName("proxy_icon_url")]
        public string ProxyIconUrl { get; set; }
    }

    public class EmbedThumbnail
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("proxy_url")]
        public string ProxyUrl { get; set; }
        [JsonPropertyName("height")]
        public int? Height { get; set; }
        [JsonPropertyName("width")]
        public int? Width { get; set; }
    }

    public  class EmbedImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("proxy_url")]
        public string ProxyUrl { get; set; }
        [JsonPropertyName("height")]
        public int? Height { get; set; }
        [JsonPropertyName("width")]
        public int? Width { get; set; }
    }

    public  class EmbedField
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("inline")]
        public bool Inline { get; set; }
    }
}
