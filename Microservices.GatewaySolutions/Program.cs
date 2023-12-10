using Microservices.GatewaySolutions.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.EnvironmentName.ToLower().ToString().Equals("production"))
    builder.Configuration.AddJsonFile("ocelot.Production.json", optional: false, reloadOnChange: true);

else
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);


builder.Services.AddOcelot(builder.Configuration);
builder.AddAppAuthentication();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseOcelot();

app.Run();
