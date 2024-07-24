using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDemo.BookStore.Authors;
using MyDemo.BookStore.Books;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyDemo.BookStore.Configurations;

public class TempEntityConfiguration : IEntityTypeConfiguration<TempEntity>
{
    public void Configure(EntityTypeBuilder<TempEntity> b)
    {
        b.ToTable(BookStoreConsts.DbTablePrefix + BookStoreConsts.DbTempTableName,
                BookStoreConsts.DbSchema);

        b.ConfigureByConvention();

        b.Property(te => te.Value)
            .HasColumnType("float");
        b.Property(te => te.ExchangeRate)
            .HasPrecision(18, 0);
        b.Property(te => te.KindOfFalName)
            .HasColumnName("Kind_Of_Fal");
        b.Property(te => te.Code)
            .HasColumnType("nvarchar(40)");

        b.HasIndex(te => te.ImportId);
        b.HasIndex(te => te.Discriminator);
    }
}
