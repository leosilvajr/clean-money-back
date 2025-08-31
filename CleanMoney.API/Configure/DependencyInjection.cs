using CleanMoney.Application.Abstractions;
using CleanMoney.Application.Services;
using CleanMoney.Domain.Repositories;
using CleanMoney.Infrastructure.Auth;
using CleanMoney.Infrastructure.Persistence;
using CleanMoney.Infrastructure.Repositories;
using CleanMoney.Shared.Settings;
using Microsoft.EntityFrameworkCore;

namespace CleanMoney.API.Configure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // JWT Settings
            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));

            // EF Core + PostgreSQL
            var conn = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(conn);
            });

            // Repositórios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompetenciaRepository, CompetenciaRepository>();
            services.AddScoped<IGrupoRepository, GrupoRepository>();
            services.AddScoped<ILancamentoCompetenciaRepository, LancamentoCompetenciaRepository>();

            // Serviços de aplicação
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICompetenciaService, CompetenciaService>();
            services.AddScoped<IGrupoService, GrupoService>();
            services.AddScoped<ILancamentoCompetenciaService, LancamentoCompetenciaService>();

            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
