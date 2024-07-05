using Volo.Abp.Modularity;

namespace MyDemo.Bookstore;

[DependsOn(
    typeof(BookstoreApplicationModule),
    typeof(BookstoreDomainTestModule)
)]
public class BookstoreApplicationTestModule : AbpModule
{

}
