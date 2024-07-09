using MyDemo.BookStore.Samples;
using Xunit;

namespace MyDemo.BookStore.EntityFrameworkCore.Domains;

[Collection(BookStoreTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<BookStoreEntityFrameworkCoreTestModule>
{

}
