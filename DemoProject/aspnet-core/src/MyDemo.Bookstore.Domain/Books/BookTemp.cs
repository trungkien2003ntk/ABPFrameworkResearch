using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyDemo.BookStore.Books;

public class BookTemp : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }

    public Guid AuthorId { get; set; }

    private BookTemp() { }

    public BookTemp(Guid id) : base(id)
    {
    }
}
