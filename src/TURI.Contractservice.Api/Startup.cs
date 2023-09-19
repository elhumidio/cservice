using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StepStone.AspNetCore.Authentication.ApiKeyHeader;
using StepStone.Extensions.Diagnostics.HealthChecks;
using StepStone.Extensions.Diagnostics.Ping;
using StepStone.Extensions.Logging.Serilog;
using StepStone.Extensions.Logging.Serilog.Options;
using StepStone.Service.Core.Options;
using TURI.Contractservice.Api.StartupExtensions;

namespace TURI.Contractservice.Api
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var config = provider.GetRequiredService<IConfiguration>();

            services.AddOptions<StepStoneServiceSettings>().Bind(config.GetSection("Service"));
            services.AddOptions<StepStoneLoggingSettings>().Bind(config.GetSection("Logging:StepStoneSerilog"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                           .AddJwtBearer(x =>
                           {
                               // Set up validation parameters from appsettings.json
                               config.GetSection("JwtBearer:TokenValidationParameters").Bind(x.TokenValidationParameters);

                               // Set up public signature from RSA 256 public key in file system
                               x.ConfigureWithPublicKeyFile(config["JwtBearer:PublicKeyFilePath"]);

                               // Set up custom authentication type, to be able to set up primary identity below
                               x.TokenValidationParameters.AuthenticationType = JwtBearerDefaults.AuthenticationScheme;
                           })
                           .AddApiKeyHeaderAuthentication(keys => config.GetSection("ApiKeyHeader:Incoming").Bind(keys))
                           .SetupPrimaryAuthenticationTypes(JwtBearerDefaults.AuthenticationScheme);

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddStepStoneSerilog();
            });
            services.AddStepStoneHealthChecks();

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureSwagger();
            services.ConfigureForwardedHeaders();
            services.AddApplicationServices(config);
            services.AddMemoryCache();
            services.AddApplicationInsightsTelemetry();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders();
                app.UseHsts();
            }

            //  app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            // app.MapControllers();
            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapGet("/_proto/", async ctx =>
                {
                    ctx.Response.ContentType = "text/plain";
                    using var fs = new FileStream(Path.Combine(env.ContentRootPath, "Proto", "contracts.proto"), FileMode.Open, FileAccess.Read);
                    using var sr = new StreamReader(fs);
                    while (!sr.EndOfStream)
                    {
                        var line = await sr.ReadLineAsync();
                        if (line != "/* >>" || line != "<< */")
                        {
                            await ctx.Response.WriteAsync(line);
                        }
                    }
                });
            });

            app.UseStepStoneHealthChecks();
            app.UsePing();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseSwaggerUI(c => c.EnableDeepLinking());
        }
    }
}
