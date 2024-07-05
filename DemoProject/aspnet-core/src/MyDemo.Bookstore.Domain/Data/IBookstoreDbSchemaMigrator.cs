using System.Threading.Tasks;

namespace MyDemo.Bookstore.Data;

public interface IBookstoreDbSchemaMigrator
{
    Task MigrateAsync();
}
