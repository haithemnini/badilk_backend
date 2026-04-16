// using BadilkBackend.src.Bootstrap;
// using BadilkBackend.src.Bootstrap;
using BadilkBackend.src.Core.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddBadilkConfiguration();
builder.Services.AddBadilkServices(builder.Configuration);

var app = builder.Build();

if (!await app.EnsureDatabaseConnectionOrStopAsync()) return;

app.MapBadilkEndpoints();
app.UseBadilkMiddleware();
app.Run();
