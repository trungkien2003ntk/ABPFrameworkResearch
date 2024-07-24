using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDemo.BookStore.Categories;

namespace MyDemo.BookStore.Departments;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(sc => sc.Code)
            .HasColumnName("Code")
            .HasColumnType("nvarchar(40)");
    }
}