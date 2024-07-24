using System;
using Volo.Abp.Application.Dtos;

namespace MyDemo.BookStore.SystemCategories;

public abstract class SystemCategoryDto : AuditedEntityDto<Guid>
{
    public string? Description { get; set; }
    
    public string? Note { get; set; }
}