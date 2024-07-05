using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDemo.Bookstore.Books;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyDemo.Bookstore.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> b)
    {
        b.ToTable(BookstoreConsts.DbTablePrefix + "Books",
                BookstoreConsts.DbSchema);
        b.ConfigureByConvention(); //auto configure for the base class props
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);
    }
}
