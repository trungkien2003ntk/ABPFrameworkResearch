using Microsoft.EntityFrameworkCore;
using MyDemo.BookStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace MyDemo.BookStore.Vats;

public class EfVatRepository : EfCoreRepository<BookStoreDbContext, Vat, Guid>, IVatRepository
{
    public EfVatRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<Vat?> FindByCodeAsync(string code)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet.FirstOrDefaultAsync(b => b.Code == code);
    }

    public async Task<List<Vat>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter = null
    )
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                b => !b.Code.IsNullOrWhiteSpace() && b.Code.Contains(filter!)
            )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }
}
