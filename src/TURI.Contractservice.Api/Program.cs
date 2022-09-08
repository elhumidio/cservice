using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using TURI.Contractservice.Api;

//var configuration = GetConfiguration();

try
{
    var host = BuildWebHost(args);
    host.Run();

    return 0;
}
catch (Exception ex)
{
    return 1;
}
finally
{
}

IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .CaptureStartupErrors(false)
        .ConfigureKestrel(options =>
        {
            //var ports = GetDefinedPorts(configuration);
            options.Listen(IPAddress.Any, 80, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                //listenOptions.UseHttps();
            });

            options.Listen(IPAddress.Any, 9988, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });

        })
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;
            config
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
        })
        .UseStartup<Startup>()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .Build();



//IConfiguration GetConfiguration()
//{
//    var builder = new ConfigurationBuilder()
//        .SetBasePath(Directory.GetCurrentDirectory())
//        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//        .AddEnvironmentVariables();

//    return builder.Build();
//}

//(int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
//{
//    var grpcPort = config.GetValue("GRPC_PORT", 9988);
//    var port = config.GetValue("PORT", 80);
//    return (port, grpcPort);
//}

public partial class Program
{

    public static string Namespace = typeof(Startup).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}
