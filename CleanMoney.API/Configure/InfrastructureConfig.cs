using CleanMoney.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMoney.API.Configure;

public static class InfrastructureConfig
{
    public static IServiceCollection AddInfrastructureConfig(this IServiceCollection services, IConfiguration config)
    {
        services.AddInfrastructure(config);
        return services;
    }
}
