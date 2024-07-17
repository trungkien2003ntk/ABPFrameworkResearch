using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyDemo.BookStore.Books;

public interface IBookRepository : IRepository<Book, Guid>
{
    Task<Book> FindByNameAsync(string name);
}
