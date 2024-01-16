using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Paddi.DemoUsages.ApiDemo.Entities;

namespace Paddi.DemoUsages.ApiDemo;

[Table("dict")]
public class Dict : ISoftDeleteEntity
{
    [Column("id")]
    public long Id { get; set; }

    [Column("key")]
    public required string Key { get; set; }

    [Column("value")]
    public required string Value { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    public override string ToString() => $"Id: {Id}, key: {Key}, value: {Value}";
}

public class DictConfig : IEntityTypeConfiguration<Dict>
{
    public void Configure(EntityTypeBuilder<Dict> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
