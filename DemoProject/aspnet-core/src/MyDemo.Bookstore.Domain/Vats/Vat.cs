using MyDemo.BookStore.Categories;
using System;

namespace MyDemo.BookStore.Vats;

public class Vat : SystemCategory
{
    private Vat() { }

    internal Vat(
        Guid id,
        string code,
        decimal value,
        string? description = null,
        string? note = null
    ) : base(id, description, note, false)
    {
        
    }

    public string? Code { get; private set; }

    public decimal Value { get; private set; }
    
    public void ChangeCode(string code)
    {
        Code = code;
    }

    public void ChangeValue(decimal value)
    {
        Value = value;
    }
}
