using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace MyDemo.Bookstore.Books;

public class BookStoreDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookStoreDataSeederContributor(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (!await _bookRepository.AnyAsync())
        {
            await _bookRepository.InsertAsync(
                new()
                {
                    Name = "1984",
                    Type = BookType.Adventure,
                    PublishDate = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
                    Price = 19.84f
                },
                autoSave: true
            );

            await _bookRepository.InsertAsync(
                new()
                {
                    Name = "The Hitchhiker's Guide to the Galaxy",
                    Type = BookType.Adventure,
                    PublishDate = new DateTime(1995, 9, 27, 0, 0, 0, DateTimeKind.Utc),
                    Price = 42.0f
                },
                autoSave: true
            );
        }
    }
}
