using Microsoft.Extensions.DependencyInjection;

namespace CleanMoney.API.Configure;

public static class ControllersConfig
{
    public static IServiceCollection AddControllersConfig(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }
}
