using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyDemo.BookStore.Vats;

public class VatConfiguration : IEntityTypeConfiguration<Vat>
{
    public void Configure(EntityTypeBuilder<Vat> builder)
    {
        builder.Property(sc => sc.Code)
            .HasColumnName("Code")
            .HasColumnType($"nvarchar({VatConsts.MaxCodeLength})");

        builder.Property(sc => sc.Value)
            .HasColumnType("float");
    }
}
