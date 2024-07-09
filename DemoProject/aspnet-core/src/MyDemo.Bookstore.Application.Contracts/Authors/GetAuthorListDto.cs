using Volo.Abp.Application.Dtos;

namespace MyDemo.BookStore.Authors;

public class GetAuthorListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
