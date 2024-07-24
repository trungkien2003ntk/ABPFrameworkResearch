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
    private readonly BookManager _bookManager;

    public BookStoreDataSeederContributor(
        IRepository<Book, Guid> bookRepository,
        IAuthorRepository authorRepository,
        AuthorManager authorManager,
        BookManager bookManager
    )
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _authorManager = authorManager;
        _bookManager = bookManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _bookRepository.GetCountAsync() > 0)
        {
            return;
        }

        await SeedBooksAndAuthors();
        await SeedSystemCategories();
    }

    private async Task SeedSystemCategories()
    {
    }

    private async Task SeedBooksAndAuthors()
    {
        var orwell = await _authorRepository.InsertAsync(
                    await _authorManager.CreateAsync(
                        "George Orwell",
                        new DateTime(1903, 06, 25, 0, 0, 0, DateTimeKind.Utc),
                        "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
                    )
                );

        var douglas = await _authorRepository.InsertAsync(
            await _authorManager.CreateAsync(
                "Douglas Adams",
                new DateTime(1952, 03, 11, 0, 0, 0, DateTimeKind.Utc),
                "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
            )
        );

        await _bookRepository.InsertAsync(
            await _bookManager.CreateAsync(
                "1984",
                BookType.Dystopia,
                new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
                19.84f,
                orwell.Id
            ),
            autoSave: true
        );

        await _bookRepository.InsertAsync(
            await _bookManager.CreateAsync(
                "The Hitchhiker's Guide to the Galaxy",
                BookType.ScienceFiction,
                new DateTime(1995, 9, 27, 0, 0, 0, DateTimeKind.Utc),
                42.0f,
                douglas.Id
            ),
            autoSave: true
        );
    }
}
