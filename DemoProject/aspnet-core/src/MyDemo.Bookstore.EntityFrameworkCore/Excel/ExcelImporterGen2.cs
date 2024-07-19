using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyDemo.BookStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Guids;

namespace MyDemo.BookStore.Excel;

[ExposeServices(typeof(IExcelImporter))]
public class ExcelImporterGen2 : IExcelImporter, ITransientDependency
{
    private List<IExcelColumnDefinition> _columnDefinitions = [];
    private readonly IDbContextProvider<BookStoreDbContext> _dbContextProvider;
    private readonly IConnectionStringResolver _connectionStringResolver;
    private readonly IGuidGenerator _guidGenerator;

    public ExcelImporterGen2(
        IDbContextProvider<BookStoreDbContext> dbContextProvider,
        IConnectionStringResolver connectionStringResolver,
        IGuidGenerator guidGenerator
    )
    {
        _dbContextProvider = dbContextProvider;
        _connectionStringResolver = connectionStringResolver;
        _guidGenerator = guidGenerator;
    }

    public void SetColumnDefinition(List<IExcelColumnDefinition> columnDefinitions)
    {
        _columnDefinitions = columnDefinitions;
    }

    public async Task ImportAsync<TTempEntity, TKey>(MemoryStream memoryStream)
        where TTempEntity : class, IEntity<TKey>
    {
        var dataTable = await ExcelHelper.ReadToDataTable(memoryStream, _columnDefinitions);
        PopulateId(dataTable);
        await BulkInsertDataTableAsync(dataTable);
        await ExecuteStoredProcedureAsync();
    }

    private void PopulateId(DataTable dataTable)
    {
        dataTable.Columns.Add("Id", typeof(Guid));

        foreach (DataRow row in dataTable.Rows)
        {
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
            DestinationTableName = "AppBooksTemp"
        };

        // Map columns and insert
        foreach (DataColumn column in dataTable.Columns)
        {
            sqlBulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
        }

        await sqlBulkCopy.WriteToServerAsync(dataTable);
        await transaction.CommitAsync();
    }

    private async Task ExecuteStoredProcedureAsync()
    {
        var dbContext = await _dbContextProvider.GetDbContextAsync();
        try
        {
            var result = await dbContext.Database.ExecuteSqlRawAsync("EXEC ValidateAndTransferBooks");
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
