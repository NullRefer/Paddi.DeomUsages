using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Paddi.DemoUsages.ApiDemo.Entities;

namespace Paddi.DemoUsages.ApiDemo;

[Table("department")]
public class Department : ISoftDeleteEntity
{
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }

    [Column("location")]
    public required string Location { get; set; }

    [Column("built")]
    public required DateTime Built { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
}

public class DepartmentConfig : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
