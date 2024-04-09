using System.Reflection;

using Hangfire;
using Hangfire.Redis.StackExchange;

using Microsoft.EntityFrameworkCore;

using Paddi.DemoUsages.ApiDemo.Cache;
using Paddi.DemoUsages.ApiDemo.Entities;
using Paddi.DemoUsages.ApiDemo.Repository;

namespace Paddi.DemoUsages.ApiDemo.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPaddiAppServices(this IServiceCollection services)
    {
        // register scoped services
        var assembly = Assembly.GetExecutingAssembly();
        var typesToRegister = assembly.ExportedTypes.Where(t => t.IsInterface && t.IsAssignableTo(typeof(IAppService)));

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
        var types = Assembly.GetExecutingAssembly()
                            .ExportedTypes
                            .Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(BackgroundService)));
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
        services.AddSingleton(sp => sp.GetRequiredService<IRedisDbProvider>().GetDatabase());

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

        var entityTypes = Assembly.GetExecutingAssembly().ExportedTypes.Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(IPaddiEntity)));
        foreach (var entityType in entityTypes)
        {
            var implementation = typeof(Repository<>).MakeGenericType(entityType);
            var serviceType = typeof(IRepository<>).MakeGenericType(entityType);
            services.AddScoped(serviceType, implementation);
        }

        return services;
    }

    public static IServiceCollection AddPaddiHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration["Redis:ConnectionString"];

        services.AddHangfireServer().AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                                                               .UseSimpleAssemblyNameTypeSerializer()
                                                                               .UseRecommendedSerializerSettings()
                                                                               .UseRedisStorage(redisConnectionString));
        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));

        return services;
    }
}
