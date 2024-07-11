using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo.BookStore.EntityFrameworkCore
{
    internal class Bookstore2DbContextFactory : IDesignTimeDbContextFactory<BookStore2DbContext>
    {
        public BookStore2DbContext CreateDbContext(string[] args)
        {
            BookStoreEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<BookStore2DbContext>()
                .UseSqlServer(configuration.GetConnectionString("Bookstore2"));

            return new BookStore2DbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MyDemo.BookStore.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
