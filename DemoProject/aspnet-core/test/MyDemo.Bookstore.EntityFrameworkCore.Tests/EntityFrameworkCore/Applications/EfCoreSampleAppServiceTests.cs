using MyDemo.Bookstore.Samples;
using Xunit;

namespace MyDemo.Bookstore.EntityFrameworkCore.Applications;

[Collection(BookstoreTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<BookstoreEntityFrameworkCoreTestModule>
{

}
