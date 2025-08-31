using CleanMoney.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanMoney.API.Configure;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApiDefaults(this WebApplication app, IWebHostEnvironment env, bool swaggerAtRoot = false)
    {
        // Aplica migrations automaticamente na subida
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (swaggerAtRoot)
                {
                    c.RoutePrefix = string.Empty; // Swagger na raiz
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanMoney API v1");
                }
            });
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
