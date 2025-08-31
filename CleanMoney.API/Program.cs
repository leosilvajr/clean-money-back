using CleanMoney.API.Configure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureConfig(builder.Configuration)   // sua configuração existente
    .AddAuthenticationConfig(builder.Configuration)
    .AddControllersConfig()
    .AddSwaggerConfig();

var app = builder.Build();

// Pipeline padrão (migrations, swagger, auth, controllers, etc.)
app.UseApiDefaults(app.Environment, swaggerAtRoot: true);

// ===== SEED DO SUPER ADMIN =====
// Comente a linha abaixo para desativar o seed automático:
await app.SeedAdminUserAsync();
// ===============================

app.Run();
