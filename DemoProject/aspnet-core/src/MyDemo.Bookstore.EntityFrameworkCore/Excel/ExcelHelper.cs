using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System;
using System.Linq;

namespace MyDemo.BookStore.Excel;

internal static class ExcelHelper
{
    public static Cell CreateCell(object value, CellValues dataType)
    {
        var cell = new Cell()
        {
            DataType = dataType
        };

        if (dataType == CellValues.Boolean)
        {
            cell.DataType = CellValues.Boolean;
            cell.CellValue = new CellValue((bool)value ? "1" : "0");
        }
        else if (dataType == CellValues.Date)
        {
            cell.DataType = new EnumValue<CellValues>(CellValues.Date);
            cell.StyleIndex = (UInt32Value)1u; // Set the style index to the built-in "Date" style
            cell.CellValue = new CellValue(value.ToString());
        }
        else if (dataType == CellValues.Number)
        {
            cell.DataType = CellValues.Number;
            cell.CellValue = new CellValue(Convert.ToDouble(value));
        }
        else
        {
            cell.DataType = CellValues.String;
            cell.CellValue = new CellValue(value.ToString());
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
}
