using Xunit;

namespace MyDemo.Bookstore.EntityFrameworkCore;

[CollectionDefinition(BookstoreTestConsts.CollectionDefinitionName)]
public class BookstoreEntityFrameworkCoreCollection : ICollectionFixture<BookstoreEntityFrameworkCoreFixture>
{

}
