using SchoolManagementAPI.Services.Configs;
using SchoolManagementAPI.Repositories;
using SchoolManagementAPI.Repositories.Repo;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Services.SMTP;
using SchoolManagementAPI.Services.CloudinaryService;
using SchoolManagementAPI.Services.Converters;

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

            services.ConfigDbContext(config);
            services.ConfigRepositories(config);
            services.ConfigAuthentication(config);
            services.ConfigDI(config);
            return services;
        }
        public static IServiceCollection ConfigDbContext(this IServiceCollection services, IConfiguration config)
        {
            DatabaseConfig databaseConfig = new DatabaseConfig();
            config.Bind("DatabaseInfo", databaseConfig);
            Console.WriteLine(databaseConfig.MyConnectionString);
            databaseConfig.SetUpDatabase();
            services.AddSingleton(databaseConfig);
            return services;
        }
        public static IServiceCollection ConfigRepositories(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ILecturerRepository, LecturerRepository>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<ISchoolClassRepository, SchoolClassRepository>();
            services.AddTransient<IFacultyRepository,FacultyRepository>();
            services.AddTransient<ISemesterRepository,SemesterRepository>();
            return services;
        }
        public static IServiceCollection ConfigAuthentication(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }
        public static IServiceCollection ConfigDI(this IServiceCollection services, IConfiguration config)
        {
            //Cloudinary
            CloudinaryConfig cloudinarySettings = new CloudinaryConfig();
            config.Bind("CloudinarySettings", cloudinarySettings);
            services.AddSingleton(cloudinarySettings);
            services.AddSingleton<CloudinaryHandler>();

            //SMTP
            SMTPConfig smtpConfigs = new SMTPConfig();
            config.Bind("SMTPConfiguration", smtpConfigs);
            services.AddSingleton(smtpConfigs);
            services.AddSingleton<EmailUtil>();

            //JSON serializing option
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter("dd/MM/yyyy"));
            });
            //Allow all hosts
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
