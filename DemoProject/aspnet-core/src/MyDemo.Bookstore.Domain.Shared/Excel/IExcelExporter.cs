using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MyDemo.BookStore.Excel;

public interface IExcelExporter
{
    Task<MemoryStream> ExportToStreamAsync<T>(List<T> data, List<IExcelColumnDefinition> columnDefinitions, string sheetName = "Sheet1")
        where T : class;
}
