using MyDemo.BookStore.Excel;
using MyDemo.BookStore.SystemCategories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.Modularity;

namespace MyDemo.BookStore.Vats;


[DependsOn(
    typeof(IExcelImporter),
    typeof(IExcelExporter),
    typeof(VatManager),
    typeof(IVatRepository)
)]
public class VatAppService : ApplicationService, IVatAppService
{
    private readonly IVatRepository _vatRepository;
    private readonly IExcelImporter _importer;
    private readonly IExcelExporter _exporter;
    private readonly VatManager _vatManager;
    private readonly List<IExcelColumnDefinition> _columnDefinitions = [];

    public VatAppService(
        IVatRepository repository,
        VatManager vatManager,
        IExcelExporter exporter,
        IExcelImporter importer
    )
    {
        _vatRepository = repository;
        _vatManager = vatManager;
        _exporter = exporter;
        _importer = importer;

        InitializeColumnDefinition();
    }

    public async Task<VatDto> CreateAsync(CreateVatDto input)
    {
        var newVat = await _vatManager.CreateAsync(
            input.Code,
            input.Value,
            input.Description,
            input.Note
        );

        var insertedVat = await _vatRepository.InsertAsync(newVat);

        return ObjectMapper.Map<Vat, VatDto>(insertedVat);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _vatRepository.DeleteAsync(id);
    }

    public async Task<VatDto> GetAsync(Guid id)
    {
        var vat = await _vatRepository.GetAsync(id);

        return ObjectMapper.Map<Vat, VatDto>(vat);
    }

    public async Task<PagedResultDto<VatDto>> GetListAsync(GetVatListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Vat.Code);
        }

        var vats = await _vatRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = await _vatRepository.GetCountAsync();

        return new PagedResultDto<VatDto>(
            totalCount,
            ObjectMapper.Map<List<Vat>, List<VatDto>>(vats)
        );
    }

    public async Task UpdateAsync(Guid id, UpdateVatDto input)
    {
        var vat = await _vatRepository.GetAsync(id);

        if (vat.Code != input.Code)
        {
            await _vatManager.ChangeCodeAsync(vat, input.Code);
        }

        vat.ChangeValue(input.Value);
        vat.Description = input.Description;
        vat.Note = input.Note;

        await _vatRepository.UpdateAsync(vat);
    }

    [RemoteService(IsEnabled = false)]
    public async Task<MemoryStream> ExportToExcelAsync()
    {
        return await _exporter.ExportToStreamAsync(await _vatRepository.GetListAsync(), _columnDefinitions, "VATs");
    }

    [RemoteService(IsEnabled = false)]
    public async Task ImportFromExcelAsync(IRemoteStreamContent file)
    {
        _importer.SetColumnDefinition(_columnDefinitions);

        using var memoryStream = new MemoryStream();
        await file.GetStream().CopyToAsync(memoryStream);
        await _importer.ImportAsync<Vat>(memoryStream);
    }

    private void InitializeColumnDefinition()
    {
        try
        {
            _columnDefinitions.AddRange([
                new ExcelColumnDefinition<string>("Code", false, value => value),
                new ExcelColumnDefinition<decimal>("Value", false, decimal.Parse),
                new ExcelColumnDefinition<string>("Description", true, value => value),
                new ExcelColumnDefinition<bool>("Deactivate", false, value => value != "0"),
                new ExcelColumnDefinition<string>("Note", true, value => value),
                new ExcelColumnDefinition<string>("Discriminator", false, value => SystemCategoryConsts.VatName)
            ]);
        }
        catch (Exception ex)
        {
            throw new BusinessException(BookStoreDomainErrorCodes.InvalidValue).WithData("detail", ex.Message);
        }
    }
}
