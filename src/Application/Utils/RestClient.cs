using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace Application.Utils
{
    public static class RestClient
    {
        public static async Task<T> Get<T>(string uri)
        {
            return await Retry.Do(async () => await GetRequest<T>(uri), TimeSpan.FromSeconds(5));
        }

        public static async Task<TOut> Post<TIn, TOut>(string uri, TIn content)
        {
            return await Retry.Do(async () => await PostRequest<TIn, TOut>(uri, content), TimeSpan.FromSeconds(5));
        }

        private static async Task<T> GetRequest<T>(string uri)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
            }
        }

        private static async Task<TOut> PostRequest<TIn, TOut>(string uri, TIn content)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(uri, serialized))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TOut>(responseBody);
                }
            }
        }

    }
}
