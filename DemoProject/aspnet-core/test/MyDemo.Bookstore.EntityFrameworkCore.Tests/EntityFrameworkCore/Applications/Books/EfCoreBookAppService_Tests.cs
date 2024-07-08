using MyDemo.Bookstore.Books;
using Xunit;

namespace MyDemo.Bookstore.EntityFrameworkCore.Applications.Books;

[Collection(BookstoreTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<BookstoreEntityFrameworkCoreTestModule>
{
}
