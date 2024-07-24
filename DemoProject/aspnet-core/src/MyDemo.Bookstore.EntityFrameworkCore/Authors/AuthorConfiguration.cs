using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDemo.BookStore.Authors;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyDemo.BookStore.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> buider)
    {
        buider.ToTable(BookStoreConsts.DbTablePrefix + "Authors",
        BookStoreConsts.DbSchema);

        buider.ConfigureByConvention();

        buider.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(AuthorConsts.MaxNameLength);

        buider.HasIndex(x => x.Name);
    }
}
