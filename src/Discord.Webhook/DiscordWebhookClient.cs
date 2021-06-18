using Discord.Webhook.Common;
using Discord.Webhook.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discord.Webhook
{
    public class DiscordWebhookClient
    {
        public ulong Id => _webhookId;
        public string Token => _webhookToken;


        private readonly ulong _webhookId;
        private readonly string _webhookToken;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public DiscordWebhookClient(ulong webhookId, string webhookToken, HttpClient httpClient, JsonSerializerOptions options = null)
        {
            _webhookId = webhookId;
            _webhookToken = webhookToken;
            _httpClient = httpClient;
            _options = options ?? new JsonSerializerOptions(){IgnoreNullValues = true, Converters =
            {
                new UlongJsonConverter()
            }};
        }

        public async Task<WebhookMessage?> SendMessageAsync(string text = null, bool isTTS = false, IEnumerable<Embed> embed = null, string username = null, string avatarUrl = null,
            RequestOptions options = null)
        {
            var args = new CreateWebhookMessageParams() { Embeds = embed?.Select(x => x.ToModel()), Content = text, Tts = isTTS, Username = username, AvatarUrl = avatarUrl };

            return await PostRequest(args);
        }

        public async Task<WebhookMessage?> SendFileAsync(IDictionary<string, Stream> files, string text = null, bool isTTS = false, IEnumerable<Embed> embed = null, string username = null, string avatarUrl = null,
            RequestOptions options = null)
        {
            var args = new CreateWebhookMessageParams() { Embeds = embed?.Select(x => x.ToModel()), Content = text, Tts = isTTS, Username = username, AvatarUrl = avatarUrl };

            return await MultiPartRequest(files, args);
        }

        public async Task<WebhookMessage> EditMessageAsync(ulong messageId, string text = null, bool isTTS = false,
            IEnumerable<Embed> embed = null, string username = null, string avatarUrl = null,
            RequestOptions options = null)
        {
            var args = new CreateWebhookMessageParams() { Embeds = embed?.Select(x => x.ToModel()), Content = text, Tts = isTTS, Username = username, AvatarUrl = avatarUrl };

            return await PatchRequest(messageId, args);
        }

        private async Task<WebhookMessage> PatchRequest(ulong messageId, CreateWebhookMessageParams args)
        {
            
            using var request = new HttpRequestMessage(HttpMethod.Patch, $"https://discord.com/api/webhooks/{_webhookId}/{_webhookToken}/messages/{messageId}?wait=true");

            request.Content = new StringContent(JsonSerializer.Serialize(args, _options));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var byteArray = await response.Content.ReadAsByteArrayAsync();

            if (byteArray.Length <= 0)
                return null;

            var msg = JsonSerializer.Deserialize<WebhookMessage>(byteArray, _options);

            return msg;
        }

        private async Task<WebhookMessage?> PostRequest(CreateWebhookMessageParams args)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"https://discord.com/api/webhooks/{_webhookId}/{_webhookToken}?wait=true");

            request.Content = new StringContent(JsonSerializer.Serialize(args, _options));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var byteArray = await response.Content.ReadAsByteArrayAsync();

            if (byteArray.Length <= 0)
                return null;

            var msg = JsonSerializer.Deserialize<WebhookMessage>(byteArray, _options);

            return msg;
        }

        private async Task<WebhookMessage?> MultiPartRequest(IDictionary<string, Stream> files, CreateWebhookMessageParams args)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"https://discord.com/api/webhooks/{_webhookId}/{_webhookToken}?wait=true");
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("Keep-Alive", "600");
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            var values = new Dictionary<string, string> { ["payload_json"] = JsonSerializer.Serialize(args, _options) };

            var content = new MultipartFormDataContent(boundary);

            foreach (var kvp in values)
                content.Add(new StringContent(kvp.Value), kvp.Key);

            int i = 1;
            foreach (var file in files)
                content.Add(new StreamContent(file.Value), $"file{i++.ToString(CultureInfo.InvariantCulture)}",
                    file.Key);

            var response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WebhookMessage>();
        }
    }
}
