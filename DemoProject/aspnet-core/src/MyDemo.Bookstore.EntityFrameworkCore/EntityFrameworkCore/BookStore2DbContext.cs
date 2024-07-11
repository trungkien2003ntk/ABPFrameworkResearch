using Microsoft.EntityFrameworkCore;
using MyDemo.BookStore.Authors;
using MyDemo.BookStore.Books;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace MyDemo.BookStore.EntityFrameworkCore;


[ConnectionStringName("Bookstore2")]
public class BookStore2DbContext : AbpDbContext<BookStore2DbContext>
{
    public BookStore2DbContext(DbContextOptions<BookStore2DbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */
        var assembly = typeof(BookStoreDbContext).Assembly;
        builder.ApplyConfigurationsFromAssembly(assembly);
    }
}
