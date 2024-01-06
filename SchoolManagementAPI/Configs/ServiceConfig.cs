using SchoolManagementAPI.Services.Configs;
using SchoolManagementAPI.Repositories;
using SchoolManagementAPI.Repositories.Repo;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Services.SMTP;
using SchoolManagementAPI.Services.CloudinaryService;
using SchoolManagementAPI.Services.Converters;
using SchoolManagementAPI.Services.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            services.AddTransient<IFacultyRepository, FacultyRepository>();
            services.AddTransient<ISemesterRepository, SemesterRepository>();
            services.AddTransient<ISchoolClassRepository, SchoolClassRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            return services;
        }
        public static IServiceCollection ConfigAuthentication(this IServiceCollection services, IConfiguration config)
        {
            TokenConfig tokenConfiguration = new TokenConfig();
            config.Bind("TokenConfiguration", tokenConfiguration);
            services.AddSingleton(tokenConfiguration);
            services.AddSingleton<TokenGenerator>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        tokenConfiguration.AccessTokenSecret)),
                    ValidIssuer = tokenConfiguration.Issuer,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(20)
                };
            });

            return services;
        }
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.AccessTokenSecret)),
        //            ValidIssuer = tokenConfiguration.Issuer,
        //            ValidAudience = tokenConfiguration.Audience,
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateIssuerSigningKey = true,
        //            ClockSkew = TimeSpan.FromMinutes(6)
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
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new TimeSpanModelBinderProvider());
            }).AddJsonOptions(options =>
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
