using MyDemo.BookStore.Categories;

namespace MyDemo.BookStore.Currencies;

public class Currency : SystemCategory
{
    public decimal? ExchangeRate { get; set; }
    
    public string? Code { get; set; }
}
