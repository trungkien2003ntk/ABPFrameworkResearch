using MyDemo.BookStore.Authors;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace MyDemo.BookStore.Books;

public class BookManager : DomainService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;

    public BookManager(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    public async Task<Book> CreateAsync(
        string name,
        BookType type,
        DateTime publishDate,
        float price,
        Guid authorId
    )
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existingBook = await _bookRepository.FindByNameAsync(name);
        
        if (existingBook != null)
        {
            // we can throw exception, or just return the existing book here based on wanted UX
            throw new BusinessException(BookStoreDomainErrorCodes.BookAlreadyExists).WithData("name", name);
        }

        var existingAuthor = await _authorRepository.FindAsync(authorId) ??
            throw new BusinessException(BookStoreDomainErrorCodes.AuthorNotExistWithId).WithData("id", authorId);
        
        return new Book(
            GuidGenerator.Create(),
            name,
            type,
            publishDate,
            price,
            existingAuthor.Id
        );
    }
}
