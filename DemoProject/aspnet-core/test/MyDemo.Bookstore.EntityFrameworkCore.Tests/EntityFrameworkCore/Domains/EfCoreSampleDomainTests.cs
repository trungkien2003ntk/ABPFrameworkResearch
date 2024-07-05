using MyDemo.Bookstore.Samples;
using Xunit;

namespace MyDemo.Bookstore.EntityFrameworkCore.Domains;

[Collection(BookstoreTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<BookstoreEntityFrameworkCoreTestModule>
{

}
