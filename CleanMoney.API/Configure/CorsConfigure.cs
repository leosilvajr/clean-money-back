using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMoney.API.Configure
{
    public static class CorsConfigure
    {
        private const string PolicyName = "CorsPolicy";

        /// <summary>
        /// Adiciona configuração de CORS liberando tudo (qualquer origem, header e método).
        /// </summary>
        public static void AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName, builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        /// <summary>
        /// Usa a política configurada no pipeline.
        /// </summary>
        public static void UseCorsPolicy(this IApplicationBuilder app)
        {
            app.UseCors(PolicyName);
        }
    }
}
