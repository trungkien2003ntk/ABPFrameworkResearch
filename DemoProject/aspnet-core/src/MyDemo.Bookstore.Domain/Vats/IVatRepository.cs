using MyDemo.BookStore.Authors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyDemo.BookStore.Vats;

public interface IVatRepository : IRepository<Vat, Guid>
{
    Task<Vat?> FindByCodeAsync(string code);

    Task<List<Vat>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter = null
    );
}
