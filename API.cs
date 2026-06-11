using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using UnityEngine;

namespace HK_Sparkler
{
    public static class SparklerAPI
    {
        [Serializable]
        public class PairResponse
        {
            public string secret;
            public string error;
        }

        public static IEnumerator Pair(HttpClient httpClient, string pairingCode, Action<PairResponse> callback)
        {
            var pairRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(httpClient.BaseAddress + "auth"),
                Headers =
                    {
                        { "pairing-code", pairingCode },
                    },
            };

            var pairResponse = httpClient.SendAsync(pairRequest);

            while (!(pairResponse.IsCompleted || pairResponse.IsFaulted))
            {
                yield return null;
            }

            var pairBody = pairResponse.Result.Content.ReadAsStringAsync();

            while (!(pairBody.IsCompleted || pairBody.IsFaulted))
            {
                yield return null;
            }

            callback(JsonUtility.FromJson<PairResponse>(pairBody.Result));
        }

        [Serializable]
        public class SparkleRequest
        {
            public float intensity;
            public float duration;
        }

        public static void Sparkle(HttpClient httpClient, string secret, float damage)
        {
            SparkleRequest req = new()
            {
                intensity = damage * 25,
                duration = 1f,
            };

            var sparkleRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(httpClient.BaseAddress + "sparkle"),
                Headers =
                {
                    { HttpRequestHeader.ContentType.ToString(), "application/json" },
                    { "secret", secret },
                },
                Content = new StringContent(JsonUtility.ToJson(req)),
            };

            httpClient.SendAsync(sparkleRequest);
        }
    }
}