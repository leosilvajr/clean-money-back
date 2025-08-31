using CleanMoney.API.Configure;
using CleanMoney.API.Configure.Examples;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginRequestExample>();
builder.Services.AddLowercaseUrls();
builder.Services
    .AddInfrastructureConfig(builder.Configuration)  
    .AddAuthenticationConfig(builder.Configuration)
    .AddControllersConfig()
    .AddSwaggerConfig();

var app = builder.Build();
app.UseApiDefaults(app.Environment, swaggerAtRoot: true);
await app.SeedAdminUserAsync();

app.Run();
