using MyDemo.BookStore.SystemCategories;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace MyDemo.BookStore.Vats;

public interface IVatAppService : IApplicationService
{
    Task<VatDto> GetAsync(Guid id);

    Task<PagedResultDto<VatDto>> GetListAsync(GetVatListDto input);

    Task<VatDto> CreateAsync(CreateVatDto input);

    Task UpdateAsync(Guid id, UpdateVatDto input);

    Task DeleteAsync(Guid id);

    Task<MemoryStream> ExportToExcelAsync();

    Task ImportFromExcelAsync(IRemoteStreamContent file);
}