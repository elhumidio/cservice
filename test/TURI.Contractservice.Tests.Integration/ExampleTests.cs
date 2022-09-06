using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net;
using TURI.Contractservice.Tests.Integration.Helpers;
using TURI.Contractservice.Tests.Integration.Helpers.Extensions;

namespace TURI.Contractservice.Tests.Integration
{
    internal sealed class ExampleTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test()
        {
            Assert.Pass();
        }

        /// <summary>
        /// This example test runs against an in-memory copy of the whole Web API
        /// created with the <see cref="ApiHostFactory" /> including default
        /// authentication mechanics.
        /// </summary>
        [Test]
        public async Task TestWebApi()
        {
            using var host = ApiHostFactory.Create();
            await host.StartAsync();

            using var httpClient = host.GetTestClient()
                .WithCandidateJwtBearer()
                .WithApiKeyHeader("APIKEY");

            var result = await httpClient.GetAsync("ping");

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("pong", await result.Content.ReadAsStringAsync());
        }
    }
}
