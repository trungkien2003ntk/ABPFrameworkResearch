using Volo.Abp.Modularity;

namespace MyDemo.Bookstore;

[DependsOn(
    typeof(BookstoreDomainModule),
    typeof(BookstoreTestBaseModule)
)]
public class BookstoreDomainTestModule : AbpModule
{

}
