using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyDemo.BookStore.Currencies;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.Property(sc => sc.ExchangeRate)
            .HasPrecision(18, 0);

        builder.Property(sc => sc.Code)
            .HasColumnName("Code")
            .HasColumnType("nvarchar(40)");
    }
}
