using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyDemo.Bookstore.Books;

public interface IBooksAppService 
    : ICrudAppService<
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto> //Used to create/update a book
{ }
