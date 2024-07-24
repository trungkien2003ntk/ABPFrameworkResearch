using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyDemo.BookStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace MyDemo.BookStore.Excel;

[ExposeServices(typeof(IExcelImporter))]
public class ExcelImporterGen2 : IExcelImporter, ITransientDependency
{
    private List<IExcelColumnDefinition> _columnDefinitions = [];
    private readonly IDbContextProvider<BookStoreDbContext> _dbContextProvider;
    private readonly IConnectionStringResolver _connectionStringResolver;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICurrentUser _currentUser;
    private readonly IClock _clock;

    public ExcelImporterGen2(
        IDbContextProvider<BookStoreDbContext> dbContextProvider,
        IConnectionStringResolver connectionStringResolver,
        IGuidGenerator guidGenerator,
        ICurrentUser currentUser,
        IClock clock
    )
    {
        _dbContextProvider = dbContextProvider;
        _connectionStringResolver = connectionStringResolver;
        _guidGenerator = guidGenerator;
        _currentUser = currentUser;
        _clock = clock;
    }

    public void SetColumnDefinition(List<IExcelColumnDefinition> columnDefinitions)
    {
        _columnDefinitions = columnDefinitions;
    }

    public async Task ImportAsync<TEntity>(MemoryStream memoryStream)
        where TEntity : class, IEntity
    {
        var dataTable = await ExcelHelper.ReadToDataTable(memoryStream, _columnDefinitions);
        PopulateCommonProperties(dataTable);
        await BulkInsertDataTableAsync(dataTable);
        await ExecuteValidationStoredProcedureAsync<TEntity>();
    }

    private void PopulateCommonProperties(DataTable dataTable)
    {
        dataTable.Columns.Add("ExtraProperties", typeof(string));
        dataTable.Columns.Add("ConcurrencyStamp", typeof(string));
        dataTable.Columns.Add("CreationTime", typeof(DateTime));
        dataTable.Columns.Add("CreatorId", typeof(Guid));
        dataTable.Columns.Add("LastModificationTime", typeof(DateTime));
        dataTable.Columns.Add("LastModifierId", typeof(Guid));
        dataTable.Columns.Add("ImportId", typeof(Guid));
        dataTable.Columns.Add("Id", typeof(Guid));
        
        
        var importId = _guidGenerator.Create();
        foreach (DataRow row in dataTable.Rows)
        {
            row["ExtraProperties"] = "{}";
            row["ConcurrencyStamp"] = _guidGenerator.Create().ToString();
            row["CreationTime"] = _clock.Now;
            row["CreatorId"] = _currentUser.Id;
            row["LastModificationTime"] = _clock.Now;
            row["LastModifierId"] = _currentUser.Id;
            row["ImportId"] = importId;
            row["Id"] = _guidGenerator.Create();
        }
    }

    private async Task BulkInsertDataTableAsync(DataTable dataTable)
    {
        var connectionString = await _connectionStringResolver.ResolveAsync("Defaults");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        using var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction)
        {
            BatchSize = 2000,
            DestinationTableName = BookStoreConsts.DbTablePrefix + BookStoreConsts.DbTempTableName,
            BulkCopyTimeout = 60
        };

        // Map columns and insert
        foreach (DataColumn column in dataTable.Columns)
        {
            sqlBulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
        }

        await sqlBulkCopy.WriteToServerAsync(dataTable);
        await transaction.CommitAsync();
    }

    private async Task ExecuteValidationStoredProcedureAsync<TEntity>()
        where TEntity : class, IEntity
    {
        var dbContext = await _dbContextProvider.GetDbContextAsync();
        try
        {
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity));
            var tableName = entityType!.GetTableName();

            dbContext.Database.SetCommandTimeout(1800);
            await dbContext.Database.ExecuteSqlRawAsync("EXEC ValidateAndTransferEntities {0}", tableName!);
            dbContext.Database.SetCommandTimeout(30);
        }
        catch (SqlException ex)
        {
            throw new BusinessException(BookStoreDomainErrorCodes.StoredProcedureError).WithData("detail", ex.Message);
        }
    }

    /* Stored Procedure
    CREATE PROCEDURE ValidateAndTransferBooks
    AS
    BEGIN
        DECLARE @ErrorMessage NVARCHAR(MAX); -- To store error messages

        BEGIN TRY -- Start the TRY block for error handling

            -- Data Validation Logic (Examples)
            IF EXISTS (SELECT 1 FROM AppBooksTemp WHERE Name IS NULL OR Name = '')
            BEGIN
                SET @ErrorMessage = 'Name is required for all books.';
                THROW 50001, @ErrorMessage, 1; -- Throw a custom error
            END

            IF EXISTS (SELECT 1 FROM AppBooksTemp WHERE PublishDate > GETDATE())
            BEGIN
                SET @ErrorMessage = 'PublishDate cannot be in the future.';
                THROW 50002, @ErrorMessage, 1; 
            END

            IF EXISTS (SELECT 1 FROM AppBooksTemp WHERE Price <= 0)
            BEGIN
                SET @ErrorMessage = 'Price must be greater than zero.';
                THROW 50003, @ErrorMessage, 1;
            END

            IF EXISTS (SELECT 1 FROM AppBooksTemp bt
                       LEFT JOIN AppAuthors a ON bt.AuthorId = a.Id
                       WHERE a.Id IS NULL) -- Check if the author exists
            BEGIN
                SET @ErrorMessage = 'Invalid AuthorId. Author not found.';
                THROW 50004, @ErrorMessage, 1;
            END

            -- If no errors, transfer to main table (Books)
            INSERT INTO AppBooks (Id, Name, Type, PublishDate, Price, AuthorId, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, ExtraProperties)
            SELECT Id, Name, Type, PublishDate, Price, AuthorId, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, ExtraProperties
            FROM AppBooksTemp;

            -- Clean Up
            TRUNCATE TABLE AppBooksTemp;

        END TRY
        BEGIN CATCH -- Catch block to handle errors
            -- Capture the error message from the CATCH block
            SET @ErrorMessage = ERROR_MESSAGE();
            -- Return the error message to your application
            SELECT @ErrorMessage AS ErrorMessage;  -- Use this in your C# code
        END CATCH
    END
     */
}
