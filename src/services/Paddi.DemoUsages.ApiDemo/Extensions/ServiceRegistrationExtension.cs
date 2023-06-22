using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Paddi.DemoUsages.ApiDemo.Cache;

namespace Paddi.DemoUsages.ApiDemo.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddPaddiAppServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        // register scoped services
        var assembly = Assembly.GetExecutingAssembly();
        var typesToRegister = assembly.ExportedTypes.Where(t => t.IsInterface && t.IsAssignableTo(typeof(IAppService))).ToList();

        foreach (var typeToRegister in typesToRegister)
        {
            var implementations = assembly.ExportedTypes.Where(t => !t.IsAbstract && t.IsAssignableTo(typeToRegister));

            foreach (var implementation in implementations)
            {
                services.AddScoped(typeToRegister, implementation);
            }
        }

        return services;
    }

    public static IServiceCollection AddPaddiHostedServices(this IServiceCollection services)
    {
        var ns = "Paddi.DemoUsages.ApiDemo.HostedServices";
        var types = Assembly.GetExecutingAssembly()
                            .ExportedTypes
                            .Where(type => type.Namespace == ns && !type.IsAbstract && type.IsAssignableTo(typeof(BackgroundService)));
        foreach (var type in types)
        {
            services.AddSingleton(typeof(IHostedService), type);
        }
        return services;
    }

    public static IServiceCollection AddPaddiRedis(this IServiceCollection services, ConfigurationManager configuration)
    {
        // register cache provider
        services.Configure<RedisOption>(configuration.GetSection("Redis"));
        services.AddSingleton<IRedisDbProvider, RedisDbProvider>();

        return services;
    }

    public static IServiceCollection AddPaddiDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        var mysqlConfig = configuration.GetSection("Mysql").Get<MysqlOption>()!;

        services.AddDbContext<ApiDemoDbContext>(options =>
        {
            options.UseMySql(mysqlConfig.ConnectionString, new MySqlServerVersion(new Version(8, 0, 0)))
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging()
                   .EnableThreadSafetyChecks();
        });
        return services;
    }

    public static WebApplication SetupEnv(this WebApplication app)
    {
        return app.InitDatabase();
    }

    public static WebApplication InitDatabase(this WebApplication app)
    {
        if (app.Environment.IsEnvironment("docker-compose"))
        {
            var dbContext = app.Services.GetRequiredService<ApiDemoDbContext>();
            dbContext.Database.Migrate();
        }

        return app;
    }
}
