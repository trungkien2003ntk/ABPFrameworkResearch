using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDemo.BookStore.Authors;
using MyDemo.BookStore.Books;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyDemo.BookStore.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> b)
    {
        b.ToTable(BookStoreConsts.DbTablePrefix + "Books",
                BookStoreConsts.DbSchema);
        b.ConfigureByConvention(); //auto configure for the base class props
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);

        b.HasOne<Author>()
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .IsRequired();
    }
}
