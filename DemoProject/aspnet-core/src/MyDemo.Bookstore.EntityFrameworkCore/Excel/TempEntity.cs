using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyDemo.BookStore.Books;

public class TempEntity : AuditedAggregateRoot<Guid>
{
    public Guid ImportId { get; set; }

    // For Books
    public string? Name { get; set; }

    public BookType? Type { get; set; }

    public DateTime? PublishDate { get; set; }

    public float? Price { get; set; }

    public Guid? AuthorId { get; set; }

    // For System Categories
    public string? Description { get; set; }

    public string? Note { get; set; }
    
    public bool? Deactivate { get; set; }

    public string? Discriminator { get; set; }

    // For Vats
    public string? Code { get; set; }

    public decimal? Value { get; set; }

    // For Currencies
    public decimal? ExchangeRate { get; set; }

    // For Kind of Fal
    public string? KindOfFalName { get; set; }

    private TempEntity() { }

    public TempEntity(Guid id) : base(id)
    {
    }
}
