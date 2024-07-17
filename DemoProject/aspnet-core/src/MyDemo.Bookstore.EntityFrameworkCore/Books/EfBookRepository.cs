using Microsoft.EntityFrameworkCore;
using MyDemo.BookStore.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyDemo.BookStore.Books;

public class EfBookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
{
    public EfBookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<Book> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet.FirstOrDefaultAsync(b => b.Name == name);
    }
}
