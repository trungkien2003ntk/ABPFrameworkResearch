using Dapper;
using Microsoft.EntityFrameworkCore;
using MyDemo.BookStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyDemo.BookStore.Authors;

public class EfCoreAuthorRepository : EfCoreRepository<BookStoreDbContext, Author, Guid>, IAuthorRepository
{
    public EfCoreAuthorRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<Author> FindByNameAsync(string name)
    {
        //// efcore way
        //var dbSet = await GetDbSetAsync();
        //return await dbSet.FirstOrDefaultAsync(author => author.Name == name);

        //// Dapper Stored Procedure
        var dbConnection = await GetDbConnectionAsync();
        var parameters = new DynamicParameters();
        parameters.Add("@name", name);

        var author = await dbConnection.QueryFirstOrDefaultAsync<Author>(
            "spGetAuthorByName",
            parameters,
            transaction: await GetDbTransactionAsync(),
            commandType: CommandType.StoredProcedure
        );

        // This is for Insert/Update/Delete Stored Procedure
        //await dbConnection.ExecuteAsync(
        //    "spGetAuthorByName",
        //    parameters,
        //    commandType: CommandType.StoredProcedure
        //);

        return author;
    }

    public async Task TestStoredProcedure(int value)
    {
        var conn = await GetDbConnectionAsync();
        var p = new DynamicParameters();
        p.Add("@Param1", value);
        p.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

        await conn.ExecuteAsync(
            "spTestErrorMessage",
            p,
            transaction: await GetDbTransactionAsync(),
            commandType: CommandType.StoredProcedure);

        
        var errorMessage = p.Get<string>("@ErrorMessage");
        if (!string.IsNullOrEmpty(errorMessage))
        {
            throw new BusinessException(BookStoreDomainErrorCodes.StoredProcedureError, "Default Message")
                .WithData("detail", errorMessage);
        }
    }

    public async Task<List<Author>> GetListAsync(int skipCount, int maxResultCount, string sorting, string? filter = null)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                author => author.Name.Contains(filter!)
                )
            .OrderBy(sorting) // this is from using System.Linq.Dynamic.Core; the sorting can be Name, Name ASC or Name DESC
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }
}
