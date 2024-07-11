using Dapper;
using MyDemo.BookStore.Authors;
using MyDemo.BookStore.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace MyDemo.BookStore.Procedure;

public class EfCoreProcedureRepository : DapperRepository<BookStore2DbContext>, IProcedureRepository, ITransientDependency
{
    public EfCoreProcedureRepository(IDbContextProvider<BookStore2DbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task RunProcedure()
    {
        var dbConnection = await GetDbConnectionAsync();
        var parameters = new DynamicParameters();
        parameters.Add("@Param1", 12);
        parameters.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size:256);

        await dbConnection.ExecuteAsync(
            "spTestErrorMessage",
            parameters,
            transaction: await GetDbTransactionAsync(),
            commandType: CommandType.StoredProcedure
        );

        var errorMsg = parameters.Get<string>("@ErrorMessage");
        if (!string.IsNullOrWhiteSpace(errorMsg))
        {
            throw new BusinessException(BookStoreDomainErrorCodes.StoredProcedureError)
                .WithData("detail", errorMsg);
        }
    }
}
