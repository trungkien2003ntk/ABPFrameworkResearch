using MyDemo.BookStore.Authors;
using Xunit;

namespace MyDemo.BookStore.EntityFrameworkCore.Applications.Authors;

[Collection(BookStoreTestConsts.CollectionDefinitionName)]
public class EfCoreAuthorAppService_Tests : AuthorAppService_Tests<BookStoreEntityFrameworkCoreTestModule>
{

}
