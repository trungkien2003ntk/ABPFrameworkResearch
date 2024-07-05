using Volo.Abp.Modularity;

namespace MyDemo.Bookstore;

public abstract class BookstoreApplicationTestBase<TStartupModule> : BookstoreTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
