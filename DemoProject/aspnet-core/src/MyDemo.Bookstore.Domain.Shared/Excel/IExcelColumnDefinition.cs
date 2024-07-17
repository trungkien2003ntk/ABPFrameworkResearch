using System;

namespace MyDemo.BookStore.Excel;

public interface IExcelColumnDefinition
{
    string ColumnName { get; set; }
    bool IsNullable { get; set; }
    Type DataType { get; }
    object ConvertValue(string value);
}
