using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace MyDemo.BookStore.Authors;

public abstract class AuthorAppService_Tests<TStartupModule> : BookStoreApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IAuthorsAppService _authorAppService;

    protected AuthorAppService_Tests()
    {
        _authorAppService = GetRequiredService<IAuthorsAppService>();
    }

    [Fact]
    public async Task Should_Get_All_Authors_Without_Any_Fitlers()
    {
        var result = await _authorAppService.GetListAsync(new GetAuthorListDto());

        result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
        result.Items.ShouldContain(author => author.Name.Contains("George"));
        result.Items.ShouldContain(author => author.Name.Contains("Douglas"));
    }

    [Fact]
    public async Task Should_Get_Filtered_Authors()
    {
        var result = await _authorAppService.GetListAsync(new GetAuthorListDto() { Filter = "George" });

        result.TotalCount.ShouldBe(1);
        result.Items.ShouldContain(author => author.Name.Contains("George"));
        result.Items.ShouldNotContain(author => author.Name.Contains("Douglas"));
    }

    [Fact]
    public async Task Should_Create_A_New_Author()
    {
        var authorName = "Edward Bellamy";
        var authorDto = await _authorAppService.CreateAsync(
            new CreateAuthorDto()
            {
                Name = authorName,
                BirthDate = new DateTime(1850, 05, 22, 0, 0, 0, DateTimeKind.Utc),
                ShortBio = "Edward Bellamy was an American author..."
            }
        );

        authorDto.Id.ShouldNotBe(Guid.Empty);
        authorDto.Name.ShouldBe(authorName);
    }

    [Fact]
    public async Task Should_Not_Allow_To_Create_Duplicate_Author()
    {
        await Should.ThrowAsync<BusinessException>(async () =>
        {
            await _authorAppService.CreateAsync(
                new CreateAuthorDto
                {
                    Name = "Douglas Adams",
                    BirthDate = DateTime.Now,
                    ShortBio = "..."
                }
            );
        });
    }

    //[Fact]
    //public async Task Should_Update_An_Author()
    //{
    //    var newName = "Edward Bellamy";
    //    // test update
    //}

    // test other methods
    /*
    GetListAsync:
        Scenario: Retrieves a list of authors.
        Test Cases:
            Should get all authors without filters. (Done)
            Should get filtered authors by name. (Done)
            Should get empty list when no matching authors are found.
            Should handle invalid filter parameters (e.g., null, empty).
    CreateAsync:
        Scenario: Creates a new author.
        Test Cases:
            Should create a new author successfully. (Done)
            Should not allow to create a duplicate author. (Done)
            Should validate input data and throw appropriate exceptions (e.g., for invalid name, birth date).
            Should handle potential concurrency issues if applicable (e.g., two users trying to create the same author simultaneously).
    UpdateAsync:
        Scenario: Updates an existing author.
        Test Cases:
            Should update an existing author successfully.
            Should not allow updating a non-existent author.
            Should validate input data and throw appropriate exceptions.
            Should handle potential concurrency issues.
    DeleteAsync:
        Scenario: Deletes an existing author.
        Test Cases:
            Should delete an existing author successfully.
            Should not allow deleting a non-existent author.
            Should handle potential foreign key constraints (e.g., if an author has books associated with them).
     */
}
