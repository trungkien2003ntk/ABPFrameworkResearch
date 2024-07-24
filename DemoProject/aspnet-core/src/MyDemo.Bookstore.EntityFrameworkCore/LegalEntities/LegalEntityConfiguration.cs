using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyDemo.BookStore.LegalEntities;

public class LegalEntityConfiguration : IEntityTypeConfiguration<LegalEntity>
{
    public void Configure(EntityTypeBuilder<LegalEntity> builder)
    {
        builder.Property(sc => sc.Code)
            .HasColumnName("Code")
            .HasColumnType("nvarchar(40)");
    }
}
