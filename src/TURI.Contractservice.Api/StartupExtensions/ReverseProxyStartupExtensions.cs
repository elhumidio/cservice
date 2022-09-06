using Microsoft.AspNetCore.HttpOverrides;

namespace TURI.Contractservice.Api.StartupExtensions
{
    internal static class ReverseProxyStartupExtensions
    {
        /// <summary>
        /// Sets up the <see cref="ForwardedHeadersOptions"/> that are sent by the proxy when the
        /// service is behind a reverse proxy. These allow the service to know how requests look
        /// like from the client side, and adapt its responses to it.
        /// <para/>
        /// Typical headers set up in a reverse proxy scenario are of the type <c>X-Forwarded-*</c>.
        /// </summary>
        /// <seealso href="https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.1"/>
        public static void ConfigureForwardedHeaders(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
        }
    }
}
