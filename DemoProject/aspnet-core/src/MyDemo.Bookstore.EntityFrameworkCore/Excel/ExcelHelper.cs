using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyDemo.BookStore.Excel;

internal static class ExcelHelper
{
    public static async Task<DataTable> ReadToDataTable(MemoryStream stream, List<IExcelColumnDefinition> columnDefinitions)
    {
        var dataTableResult = new DataTable();

        foreach (var columnDefinition in columnDefinitions)
        {
            dataTableResult.Columns.Add(columnDefinition.ColumnName, columnDefinition.DataType);
        }

        // Open the Excel file and iterate over rows
        using var spreadsheetDocument = SpreadsheetDocument.Open(stream, false);
        var workbookPart = spreadsheetDocument.WorkbookPart;
        var worksheetPart = workbookPart.WorksheetParts.Last();
        var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
        var rows = sheetData.Descendants<Row>().ToList();

        if (rows.Count < 2) return dataTableResult; // Ensure there are at least two rows (header + data)

        // Get string table to retrieve excel's shared strings
        var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()?.SharedStringTable!;

        // Extract column names from the header row
        var headerRow = rows.First();

        var columnMappings = new Dictionary<int, IExcelColumnDefinition>();

        foreach (Cell cell in headerRow.Descendants<Cell>())
        {
            var columnName = GetCellValue(cell, stringTable);
            var columnIndex = CellReferenceToIndex(cell);
            var columnDefinition = columnDefinitions.FirstOrDefault(cd => cd.ColumnName == columnName);

            if (columnDefinition != null)
            {
                columnMappings[columnIndex] = columnDefinition;
            }
        }

        var dataRows = rows.Skip(1);
        foreach (Row row in dataRows)
        {
            var dataRow = dataTableResult.NewRow();

            foreach (Cell cell in row.Descendants<Cell>())
            {
                var columnIndex = CellReferenceToIndex(cell);
                if (columnMappings.TryGetValue(columnIndex, out var columnDefinition))
                {
                    var cellValue = GetCellValue(cell, stringTable);
                    dataRow[columnDefinition.ColumnName] = string.IsNullOrEmpty(cellValue) && columnDefinition.IsNullable
                        ? DBNull.Value
                        : columnDefinition.ConvertValue(cellValue);
                }
            }

            dataTableResult.Rows.Add(dataRow);
        }


        return dataTableResult;
    }

    public static Cell CreateCell(object? value, Type dataType)
    {
        var cell = new Cell();

        var columnTypeToCellDataTypeMap = new Dictionary<Type, CellValues>
        {
            { typeof(bool), CellValues.Boolean },
            { typeof(byte), CellValues.Number },
            { typeof(char), CellValues.String },
            { typeof(string), CellValues.String },
            { typeof(DateTime), CellValues.Date },
            { typeof(double), CellValues.Number },
            { typeof(decimal), CellValues.Number },
            { typeof(short), CellValues.Number },
            { typeof(int), CellValues.Number },
            { typeof(long), CellValues.Number },
            { typeof(sbyte), CellValues.Number },
            { typeof(float), CellValues.Number },
            { typeof(ushort), CellValues.Number },
            { typeof(uint), CellValues.Number },
            { typeof(ulong), CellValues.Number },
        };

        if (columnTypeToCellDataTypeMap.TryGetValue(dataType, out var cellDataType))
        {
            cell.DataType = new EnumValue<CellValues>(cellDataType);
            
            if (value == null)
            {
                return cell;
            }

            if (cell.DataType == CellValues.Date)
            {
                cell.CellValue = new CellValue(((DateTime)value).ToOADate().ToString());
            }
            else if (cell.DataType == CellValues.Boolean)
            {
                cell.CellValue = new CellValue((bool)value ? "1" : "0");
            }
            else
            {
                cell.CellValue = new CellValue(value.ToString());
            }
        }

        return cell;
    }

    public static string GetCellValue(Cell cell, SharedStringTable stringTable)
    {
        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        {
            return stringTable.ElementAt(int.Parse(cell.InnerText)).InnerText;
        }
        return cell.InnerText;
    }

    public static int GetColumnIndex(Cell cell)
    {
        string cellReference = cell.CellReference?.Value;
        if (string.IsNullOrEmpty(cellReference))
        {
            return -1;
        }

        string columnLetters = new string(cellReference.TakeWhile(char.IsLetter).ToArray());
        int columnIndex = 0;
        foreach (char letter in columnLetters)
        {
            columnIndex = (columnIndex * 26) + (letter - 'A' + 1);
        }
        return columnIndex - 1; // Convert to zero-based index
    }

    public static int CellReferenceToIndex(Cell cell)
    {
        int index = -1;
        string reference = cell.CellReference.ToString().ToUpper();
        foreach (char ch in reference)
        {
            if (Char.IsLetter(ch))
            {
                int value = (int)ch - (int)'A';
                index = (index + 1) * 26 + value;
            }
            else
                return index;
        }
        return index;
    }
}
