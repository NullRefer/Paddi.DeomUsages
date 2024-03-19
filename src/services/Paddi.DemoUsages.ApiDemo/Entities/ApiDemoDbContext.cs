using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Paddi.DemoUsages.ApiDemo.Entities;

namespace Paddi.DemoUsages.ApiDemo;

public class ApiDemoDbContext : DbContext
{
    public ApiDemoDbContext(DbContextOptions<ApiDemoDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCharSet("utf8mb4");

        var entityTypes = Assembly.GetExecutingAssembly().ExportedTypes.Where(e => !e.IsAbstract && e.IsAssignableTo(typeof(IPaddiEntity)));
        foreach (var entityType in entityTypes)
        {
            modelBuilder.Entity(entityType);
        }
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public async Task<int> SoftDeleteAsync<T>(T entity) where T : class, ISoftDeleteEntity
    {
        entity.IsDeleted = true;
        Set<T>().Update(entity);
        return await SaveChangesAsync();
    }
}
