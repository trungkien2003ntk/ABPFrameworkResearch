using MyDemo.BookStore.SystemCategories;
using System.ComponentModel.DataAnnotations;

namespace MyDemo.BookStore.Vats;

public class CreateVatDto : CreateSystemCategoryDto
{
    [Required]
    [StringLength(VatConsts.MaxCodeLength)]
    public string Code { get; set; } = string.Empty;

    [Required]
    public decimal Value { get; set; }
}
