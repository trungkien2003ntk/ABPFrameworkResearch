using System;
using Volo.Abp.DependencyInjection;

namespace MyDemo.BookStore.Excel;

public class ExcelColumnDefinition<T> : IExcelColumnDefinition, ITransientDependency
{
    public string ColumnName { get; set; }
    public bool IsNullable { get; set; }
    public Type DataType => typeof(T);
    public Func<string, T> ValueConverter { get; set; } // Convert string from Excel to entity type

    // Constructor
    public ExcelColumnDefinition(string columnName, bool isNullable, Func<string, T> valueConverter)
    {
        ColumnName = columnName;
        IsNullable = isNullable;
        ValueConverter = valueConverter;
    }

    public object ConvertValue(string value)
    {
        return ValueConverter(value);
    }
}
