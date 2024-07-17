using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyDemo.BookStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Guids;

namespace MyDemo.BookStore.Excel;

[ExposeServices(typeof(IExcelImporter))]
public class ExcelImporter : IExcelImporter, ITransientDependency
{
    private List<IExcelColumnDefinition> _columnDefinitions;
    private const int BATCH_SIZE = 2000;
    private readonly IDbContextProvider<BookStoreDbContext> _dbContextProvider;
    private IGuidGenerator guidGenerator;

    public ExcelImporter(IDbContextProvider<BookStoreDbContext> dbContextProvider, IGuidGenerator guidGenerator)
    {
        _dbContextProvider = dbContextProvider;
        this.guidGenerator = guidGenerator;
    }

    public void SetColumnDefinition(List<IExcelColumnDefinition> columnDefinitions)
    {
        _columnDefinitions = columnDefinitions;
    }

    public async Task ImportAsync<TTempEntity, TKey>(MemoryStream memoryStream)
        where TTempEntity : class, IEntity<TKey>
    {
        // Open the Excel file and iterate over rows
        using var spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, false);
        var workbookPart = spreadsheetDocument.WorkbookPart;
        var worksheetPart = workbookPart.WorksheetParts.Last();
        var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

        // Get string table to retrieve excel's shared strings
        var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()?.SharedStringTable;

        var rows = sheetData.Descendants<Row>().ToList();
        if (rows.Count < 2) return; // Ensure there are at least two rows (header + data)

        // Extract column names from the header row
        var headerRow = rows.First();
        var columnMappings = new Dictionary<int, string>(); // Maps column index to column name
        foreach (var cell in headerRow.Descendants<Cell>())
        {
            var columnName = ExcelHelper.GetCellValue(cell, stringTable);
            var columnIndex = ExcelHelper.GetColumnIndex(cell);
            if (!string.IsNullOrEmpty(columnName))
            {
                columnMappings[columnIndex] = columnName.Trim().Replace(" ", "");
            }
        }

        // Skip the header row and iterate over data rows
        var dataRows = rows.Skip(1);
        List<TTempEntity> batch = new List<TTempEntity>();
        int rowCount = 0;
        int totalRowCount = dataRows.Count();
        var dbContext = await _dbContextProvider.GetDbContextAsync();

        foreach (var row in dataRows)
        {
            // 1. Create a new TEntity instance
            TTempEntity entity = (TTempEntity)Activator.CreateInstance(typeof(TTempEntity), guidGenerator.Create());
            
            // 2. Populate common audited properties
            
            // 3. Map values from Excel cells to entity properties using columnMappings
            foreach (var cell in row.Descendants<Cell>())
            {
                var columnIndex = ExcelHelper.GetColumnIndex(cell);
                if (columnMappings.TryGetValue(columnIndex, out var columnName))
                {
                    var cellValue = ExcelHelper.GetCellValue(cell, stringTable);

                    var columnDefinition = _columnDefinitions.FirstOrDefault(cd => cd.ColumnName == columnName);
                    if (columnDefinition != null)
                    {
                        if (!string.IsNullOrEmpty(cellValue) || columnDefinition.IsNullable)
                        {
                            var property = typeof(TTempEntity).GetProperty(columnDefinition.ColumnName);
                            property?.SetValue(entity, columnDefinition.ConvertValue(cellValue));
                        }
                    }
                }
            }

            // 4. Add the entity to a batch
            dbContext.Attach(entity);
            dbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            rowCount++;

            if (batch.Count >= BATCH_SIZE || rowCount == totalRowCount)
            {   
                await dbContext.SaveChangesAsync();
                batch.Clear();
            }
        }

        // exec ValidateAndTransferBooks
        try
        {
            var result = await dbContext.Database.ExecuteSqlRawAsync("EXEC ValidateAndTransferBooks");
        }
        catch (SqlException ex)
        {
            throw new BusinessException(BookStoreDomainErrorCodes.StoredProcedureError).WithData("detail", ex.Message);
        }
    }


    private string GetColumnName(Cell cell)
    {
        // Ensure the cell has a reference
        string cellReference = cell?.CellReference?.Value;
        if (string.IsNullOrEmpty(cellReference))
        {
            return null;
        }

        // Extract the column letters from the cell reference
        string columnLetters = new string(cellReference.TakeWhile(char.IsLetter).ToArray());

        // Convert the column letters to a column index (1-based)
        int columnIndex = 0;
        foreach (char letter in columnLetters)
        {
            columnIndex = (columnIndex * 26) + (letter - 'A' + 1);
        }

        // Adjust for zero-based indexing
        columnIndex--;

        // Ensure the column index is within the range of your column definitions
        if (columnIndex < 0 || columnIndex >= _columnDefinitions.Count)
        {
            return null;
        }

        // Retrieve the column name from your column definitions
        return _columnDefinitions.ElementAt(columnIndex)?.ColumnName;
    }
    // Private helper methods for:
    // - Populating common audited properties
    // - Mapping values (using ValueConverter from ExcelColumnDefinition)
    // - Batch insertion to the temporary table
}
