namespace Paddi.DemoUsages.ApiDemo.Entities;

public interface IPaddiEntity
{
}

public interface ISoftDeleteEntity : IPaddiEntity
{
    bool IsDeleted { get; set; }
}

