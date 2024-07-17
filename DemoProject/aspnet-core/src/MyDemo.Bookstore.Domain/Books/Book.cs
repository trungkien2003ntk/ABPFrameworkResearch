using MyDemo.BookStore.Authors;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyDemo.BookStore.Books;

public class Book : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }    

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }

    public Guid AuthorId { get; set; }

    protected Book()
    {

    }
    
    internal Book(Guid id, string name, BookType type, DateTime publishedDate, float price, Guid authorId)
    {
        Id = id;
        SetName(name);
        Type = type;
        PublishDate = publishedDate;
        Price = price;
        AuthorId = authorId;
    }

    private void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), BookConsts.MaxNameLength);
    }
}
