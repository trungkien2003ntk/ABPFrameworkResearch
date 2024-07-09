using System.Threading.Tasks;

namespace MyDemo.BookStore.Data;

public interface IBookStoreDbSchemaMigrator
{
    Task MigrateAsync();
}
