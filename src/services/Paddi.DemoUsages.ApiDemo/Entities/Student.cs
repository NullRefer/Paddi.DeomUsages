using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Paddi.DemoUsages.ApiDemo.Entities;

namespace Paddi.DemoUsages.ApiDemo;

[Table("student")]
public class Student : ISoftDeleteEntity
{
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }

    [Column("age")]
    public required int Age { get; set; }

    [Column("birth")]
    public required DateTime Birth { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
}

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
