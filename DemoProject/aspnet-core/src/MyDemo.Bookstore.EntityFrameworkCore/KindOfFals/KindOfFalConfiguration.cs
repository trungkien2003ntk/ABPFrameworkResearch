using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDemo.BookStore.Categories;

namespace MyDemo.BookStore.KindOfFals;

public class KindOfFalConfiguration : IEntityTypeConfiguration<KindOfFal>
{
    public void Configure(EntityTypeBuilder<KindOfFal> builder)
    {
        builder.Property(sc => sc.KindOfFalName)
            .HasColumnName("Kind_Of_Fal")
            .HasColumnType("nvarchar(max)");
    }
}
