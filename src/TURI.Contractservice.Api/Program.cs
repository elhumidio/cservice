using TURI.Contractservice.Api;

var builder = WebApplication.CreateBuilder(args);

Startup.ConfigureServices(builder.Services);

var app = builder.Build();

Startup.Configure(app);

app.Run();
