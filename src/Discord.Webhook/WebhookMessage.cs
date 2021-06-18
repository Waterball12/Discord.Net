using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discord.Webhook
{
    /// <summary>
    /// Entity that rapresent a generic webhook message
    /// </summary>
    /// <remarks>
    /// STILL NOT FINISHED
    /// </remarks>
    public record WebhookMessage
    {
        [JsonPropertyName("id")]
        public ulong Id { get; init; }

        [JsonPropertyName("channel_id")]
        public ulong ChannelId { get; init; }
    }
}
