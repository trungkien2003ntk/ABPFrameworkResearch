using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using System.IO;
using Volo.Abp.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace MyDemo.BookStore.Excel;

[ExposeServices(typeof(IExcelExporter))]
public class ExcelExporter : IExcelExporter, ITransientDependency
{
    public async Task<MemoryStream> ExportToStreamAsync<T>(List<T> data, List<IExcelColumnDefinition> columnDefinitions, string sheetName = "Sheet1") where T : class
    {
        var memoryStream = new MemoryStream();

        using (var spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook { Sheets = new Sheets() };

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            var sheetID = sheets.Elements<Sheet>().Any() ? sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1 : 1;
            var relationshipId = workbookPart.GetIdOfPart(worksheetPart);

            var sheet = new Sheet { Id = relationshipId, SheetId = sheetID, Name = sheetName };
            sheets.Append(sheet);

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            sheetData = AppendHeaderRow<T>(sheetData, columnDefinitions);
            sheetData = await AppendDataRows<T>(data, sheetData, columnDefinitions);

            workbookPart.Workbook.Save();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    private SheetData AppendHeaderRow<T>(SheetData sheetData, List<IExcelColumnDefinition> columnDefinitions) where T : class
    {
        var headerRow = new Row();

        foreach (var columnDefinition in columnDefinitions)
        {
            var cell = ExcelHelper.CreateCell(columnDefinition.ColumnName, typeof(string));
            headerRow.AppendChild(cell);
        }

        sheetData.AppendChild(headerRow);
        return sheetData;
    }

    private async Task<SheetData> AppendDataRows<T>(
        List<T> data,
        SheetData sheetData,
        List<IExcelColumnDefinition> columnDefinitions
    ) where T : class
    {
        foreach (var item in data)
        {
            var dataRow = new Row();

            foreach (var columnDefinition in columnDefinitions)
            {
                var property = typeof(T).GetProperty(columnDefinition.ColumnName);
                var value = property?.GetValue(item);

                var cell = ExcelHelper.CreateCell(value, columnDefinition.DataType);
                dataRow.AppendChild(cell);
            }

            sheetData.AppendChild(dataRow);
        }

        return sheetData;
    }
}
