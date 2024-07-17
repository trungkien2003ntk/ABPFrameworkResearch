using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyDemo.BookStore.Categories;

public class SystemCategory : AuditedAggregateRoot<Guid>
{
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public bool Deactivate { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string? Discriminator { get; set; }
    public string? Kind_Of_Val { get; set; }
    public float Value { get; set; }
}
