using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDemo.BookStore.Authors;
using MyDemo.BookStore.Books;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyDemo.BookStore.Configurations;

public class BookTempConfiguration : IEntityTypeConfiguration<BookTemp>
{
    public void Configure(EntityTypeBuilder<BookTemp> b)
    {
        b.ToTable(BookStoreConsts.DbTablePrefix + "BooksTemp",
                BookStoreConsts.DbSchema);
        b.ConfigureByConvention(); //auto configure for the base class props
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);
    }
}
