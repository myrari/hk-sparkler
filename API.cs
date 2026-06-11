using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace HK_Sparkler
{
    public static class SparklerAPI
    {
        public record class PairResponse(
            string? Secret,
            string? Error
        );

        public static async Task<PairResponse> Pair(HttpClient httpClient, string pairingCode)
        {
            var pairRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(httpClient.BaseAddress + "/auth"),
                Headers =
                {
                    { "pairing-code", pairingCode },
                },
            };

            var pairResponse = await httpClient.SendAsync(pairRequest);

            return await pairResponse.Content.ReadFromJsonAsync<PairResponse>();
        }

        public record class SparkleRequest(
            float Intensity,
            float Duration
        );

        public static async Task<int> Sparkle(HttpClient httpClient, string secret, float damage)
        {
            float intensity = damage * 25;

            var sparkleRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(httpClient.BaseAddress + "/sparkle"),
                Headers =
                {
                    { HttpRequestHeader.ContentType.ToString(), "application/json" },
                    { "secret", secret },
                },
                Content = new StringContent(JsonSerializer.Serialize(new SparkleRequest(intensity, 1.0f))),
            };

            return (int)(await httpClient.SendAsync(sparkleRequest)).StatusCode;
        }
    }
}