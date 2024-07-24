using Volo.Abp.Application.Dtos;

namespace MyDemo.BookStore.Vats;

public class GetVatListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
