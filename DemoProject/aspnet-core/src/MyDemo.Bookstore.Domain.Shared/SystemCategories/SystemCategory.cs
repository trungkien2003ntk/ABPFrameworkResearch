using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyDemo.BookStore.Categories;

public abstract class SystemCategory : AuditedAggregateRoot<Guid>
{
    protected SystemCategory() { }

    protected SystemCategory(Guid id, string? description = null, string? note = null, bool deactivate = false) : base(id)
    {
        Description = description;
        Note = note;
        Deactivate = deactivate;
    }

    public string? Description { get; set; }
    
    public string? Note { get; set; }
    
    public bool Deactivate { get; set; } 
}
