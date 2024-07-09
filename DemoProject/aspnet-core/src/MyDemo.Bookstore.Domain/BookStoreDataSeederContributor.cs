using MyDemo.BookStore.Authors;
using MyDemo.BookStore.Books;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace MyDemo.BookStore;

public class BookStoreDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Book, Guid> _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly AuthorManager _authorManager;

    public BookStoreDataSeederContributor(
        IRepository<Book, Guid> bookRepository,
        IAuthorRepository authorRepository,
        AuthorManager authorManager
    )
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _authorManager = authorManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedBooks();
        await SeedAuthors();
    }

    private async Task SeedBooks()
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

    private async Task SeedAuthors()
    {
        if (await _authorRepository.GetCountAsync() <= 0)
        {
            await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "George Orwell",
                    new DateTime(1903, 06, 25, 0, 0, 0, DateTimeKind.Utc),
                    "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
                )
            );

            await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "Douglas Adams",
                    new DateTime(1952, 03, 11, 0, 0, 0, DateTimeKind.Utc),
                    "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
                )
            );
        }
    }
}
