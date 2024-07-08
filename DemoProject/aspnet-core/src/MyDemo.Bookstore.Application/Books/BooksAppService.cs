using MyDemo.Bookstore.Permissions;
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
        GetPolicyName = BookstorePermissions.Books.Default;
        GetListPolicyName = BookstorePermissions.Books.Default;
        CreatePolicyName = BookstorePermissions.Books.Create;
        UpdatePolicyName = BookstorePermissions.Books.Edit;
        DeletePolicyName = BookstorePermissions.Books.Delete;
    }
}
