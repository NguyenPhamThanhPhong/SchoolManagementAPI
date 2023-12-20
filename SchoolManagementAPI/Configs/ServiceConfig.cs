using SchoolManagementAPI.Services.Configs;
using SchoolManagementAPI.Repositories;
using SchoolManagementAPI.Repositories.Repo;
using SchoolManagementAPI.Repositories.Interfaces;

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
            DatabaseConfig databaseConfig = new DatabaseConfig();
            config.Bind("DatabaseInfo", databaseConfig);
            databaseConfig.SetUpDatabase();
            services.AddSingleton(databaseConfig);
            return services;
        }
        public static IServiceCollection ConfigRepositories(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<ILecturerRepository,LecturerRepository>();
            services.AddTransient<IAdminRepository,AdminRepository>();
            services.AddSingleton<IStudentRepository, StudentRepository>();

            //services.AddSingleton<IScheduleAggregationRepository,ScheduleAggregationRepository>();
            //services.AddSingleton<ISubjectRepository, SubjectRepository>();
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
