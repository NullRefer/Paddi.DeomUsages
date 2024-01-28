using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Paddi.DemoUsages.ApiDemo.Services.IServices;

using Testcontainers.MySql;
using Testcontainers.Redis;

namespace Paddi.DemoUsages.ApiDemo.IntegrationTests;

public class ApiDemoWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _dbContainer = new MySqlBuilder().Build();
    private readonly RedisContainer _redisContainer = new RedisBuilder().WithHostname("redis-integration-test").Build();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        using (var scope = host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDemoDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.Single(s => s.ServiceType == typeof(DbContextOptions<ApiDemoDbContext>));
            services.Remove(dbContextDescriptor);

            //services.AddSingleton<DbConnection>(container =>
            //{
            //});

            //services.AddDbContext<ApiDemoDbContext>((container, options) =>
            //{
            //    var connection = container.GetRequiredService<DbConnection>();
            //    options.UseMySql(connection);
            //});
            services.AddDbContext<ApiDemoDbContext>(optionsBuilder =>
            {
                var connectionString = _dbContainer.GetConnectionString();
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
                {
                    options.CommandTimeout(10);
                    options.EnableIndexOptimizedBooleanColumns();
                })
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
            });
        });

        builder.UseEnvironment("integration-test");
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}

