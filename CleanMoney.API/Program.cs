using CleanMoney.API.Configure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureConfig(builder.Configuration)
    .AddAuthenticationConfig(builder.Configuration)
    .AddControllersConfig()
    .AddSwaggerConfig();

var app = builder.Build();

app.UseApiDefaults(app.Environment, swaggerAtRoot: true);

app.Run();
