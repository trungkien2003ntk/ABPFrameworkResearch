using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDemo.BookStore.Categories;
using MyDemo.BookStore.Currencies;
using MyDemo.BookStore.Departments;
using MyDemo.BookStore.ExpenseCodes;
using MyDemo.BookStore.KindOfFals;
using MyDemo.BookStore.LegalEntities;
using MyDemo.BookStore.Vats;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyDemo.BookStore.SystemCategories;

public class SystemCategoryConfiguration : IEntityTypeConfiguration<SystemCategory>
{
    public void Configure(EntityTypeBuilder<SystemCategory> builder)
    {
        builder.ToTable(BookStoreConsts.DbTablePrefix + "SystemCategories", BookStoreConsts.DbSchema);
        
        builder.ConfigureByConvention();

        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<SystemCategory>(SystemCategoryConsts.SystemCategoryName)
            .HasValue<KindOfFal>(SystemCategoryConsts.KindOfFalName)
            .HasValue<Currency>(SystemCategoryConsts.CurrencyName)
            .HasValue<Vat>(SystemCategoryConsts.VatName)
            .HasValue<Department>(SystemCategoryConsts.DepartmentName)
            .HasValue<LegalEntity>(SystemCategoryConsts.LegalEntityName)
            .HasValue<ExpenseCode>(SystemCategoryConsts.ExpenseCodeName);
    }
}
