using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyDemo.Bookstore.Books;

public class BooksAppService : 
    CrudAppService<
        Book, 
        BookDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateBookDto>, 
    IBooksAppService
{
    public BooksAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {

    }
}
