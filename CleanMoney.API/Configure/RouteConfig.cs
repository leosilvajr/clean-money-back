namespace CleanMoney.API.Configure
{
    public static class RouteConfig
    {
        /// <summary>
        /// Configura URLs e query strings para sempre ficarem em minúsculo.
        /// </summary>
        public static void AddLowercaseUrls(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true; // opcional
            });
        }
    }
}
