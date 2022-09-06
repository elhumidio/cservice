using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace TURI.Contractservice.Api.StartupExtensions
{
    /// <summary>
    /// An internal extensions class for abstracting <see cref="Startup"/> configurations in
    /// shorter, readable commands.
    /// </summary>
    internal static class SwaggerStartupExtensions
    {
        /// <summary>
        /// Configures Swagger (Open API) symbol generation.
        /// </summary>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service API", Version = "v1" }));
        }

        /// <summary>
        /// Adds a Swagger <see cref="OpenApiServer"/> to the Swagger output that considers the
        /// <c>X-Forwarded-Prefix</c> header to build the base path.
        /// </summary>
        public static void AddForwardedHeadersServer(this SwaggerOptions options)
        {
            options.PreSerializeFilters.Add((doc, request) =>
            {
                var forwardedScheme = request.Headers["X-Forwarded-Proto"].ToString();
                if (!string.IsNullOrEmpty(forwardedScheme))
                {
                    request.Scheme = forwardedScheme;
                }
                var serverUrl = UriHelper.BuildAbsolute(
                    request.Scheme,
                    request.Host,
                    request.PathBase = request.Headers["X-Forwarded-Prefix"].ToString());

                doc.Servers.Add(new OpenApiServer { Url = serverUrl });
            });
        }
    }
}
