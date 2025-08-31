using CleanMoney.API.Configure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureConfig(builder.Configuration)   // sua configuração existente
    .AddAuthenticationConfig(builder.Configuration)
    .AddControllersConfig()
    .AddSwaggerConfig();

var app = builder.Build();

app.UseApiDefaults(app.Environment, swaggerAtRoot: true);

// ===== SEED DO SUPER ADMIN =====
await app.SeedAdminUserAsync();

app.Run();
