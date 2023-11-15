namespace SchoolManagementAPI.Configs
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddServicesConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSignalR();
            return services;
        }
        public static IServiceCollection ConfigDbContext(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }
        public static IServiceCollection ConfigRepositories(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }
        public static IServiceCollection ConfigAuthentication(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }
        public static IServiceCollection ConfigDI(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }
    }
}
