using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TURI.Contractservice.Api;

namespace TURI.Contractservice.Tests.Integration.Helpers
{
    internal static class ApiHostFactory
    {
        /// <summary>
        /// Initializes a new web host using the <see cref="Startup"/> from the API project in the
        /// solution and the given overrides for <paramref name="url"/> and <paramref name="testServices"/>.
        /// </summary>
        /// <param name="configureDelegate">
        /// An optional action to override the host's <see cref="IConfiguration"/> through a <see cref="IConfigurationBuilder"/>.
        /// </param>
        /// <param name="testServices">
        /// An action to configure overrides for the web host's <see cref="IServiceCollection"/>.
        /// Unless test overrides are provided, the defaults set in the <see cref="Helpers"/> class
        /// will be used. <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1"/>
        /// </param>
        public static IHost Create(
            Action<IConfigurationBuilder>? configureDelegate = null,
            Action<IServiceCollection>? testServices = null)
        {
            var builder = WebApplication.CreateBuilder();

            if (configureDelegate != null)
                builder.WebHost.ConfigureAppConfiguration(configureDelegate);

            builder.WebHost.UseTestServer();

            Startup.ConfigureServices(builder.Services);

            if (testServices != null)
                builder.WebHost.ConfigureTestServices(testServices);

            var app = builder.Build();

            //  Startup.Configure(app
            Startup.Configure(app, app.Environment, null);

            return app;
        }
    }
}
