using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyDemo.BookStore.ExpenseCodes;

public class ExpenseCodeConfiguration : IEntityTypeConfiguration<ExpenseCode>
{
    public void Configure(EntityTypeBuilder<ExpenseCode> builder)
    {
        builder.Property(sc => sc.Code)
            .HasColumnName("Code")
            .HasColumnType("nvarchar(40)");
    }
}
