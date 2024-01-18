using System.Data.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Paddi.DemoUsages.ApiDemo.IntegrationTests;

public class ApiDemoWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
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
        });

        builder.UseEnvironment("development");
    }
}

