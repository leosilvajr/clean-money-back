using CleanMoney.API.Configure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureConfig(builder.Configuration)   // sua configura��o existente
    .AddAuthenticationConfig(builder.Configuration)
    .AddControllersConfig()
    .AddSwaggerConfig();

var app = builder.Build();

// Pipeline padr�o (migrations, swagger, auth, controllers, etc.)
app.UseApiDefaults(app.Environment, swaggerAtRoot: true);

// ===== SEED DO SUPER ADMIN =====
// Comente a linha abaixo para desativar o seed autom�tico:
await app.SeedAdminUserAsync();
// ===============================

app.Run();
