using StepStone.AspNetCore.Authentication.ApiKeyHeader;
using System.Net.Http.Headers;

namespace TURI.Contractservice.Tests.Integration.Helpers.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient WithCandidateJwtBearer(this HttpClient client)
        {
            var token = JwtTokenGenerator.GenerateStepStoneDEToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        public static HttpClient WithApiKeyHeader(this HttpClient client, string apiKey)
        {
            client.DefaultRequestHeaders.Add(ApiKeyDefaults.HeaderName, apiKey);
            return client;
        }
    }
}
