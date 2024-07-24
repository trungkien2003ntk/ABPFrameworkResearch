using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace MyDemo.BookStore.Vats;

public class VatManager : DomainService
{
    private readonly IVatRepository _vatRepository;

    public VatManager(IVatRepository vatRepository)
    {
        _vatRepository = vatRepository;
    }

    public async Task<Vat> CreateAsync(
        string code,
        decimal value,
        string? description = null,
        string? note = null
    )
    {
        Check.NotNullOrWhiteSpace(code, nameof(code));
        Check.NotNull(value, nameof(value));

        var existingVat = await _vatRepository.FindByCodeAsync(code);
        if (existingVat != null)
        {
            throw new BusinessException(BookStoreDomainErrorCodes.VatWithCodeAlreadyExists).WithData("code", code);
        }

        return new Vat(
            GuidGenerator.Create(),
            code,
            value,
            description,
            note
        );
    }

    public async Task ChangeCodeAsync(
        Vat vat,
        string newCode
    )
    {
        Check.NotNull(vat, nameof(vat));
        Check.NotNullOrWhiteSpace(newCode, nameof(newCode));

        var vatWithNewCode = await _vatRepository.FindByCodeAsync(newCode);
        bool vatWithNewCodeAlreadyExists = vatWithNewCode != null && vatWithNewCode.Id != vat.Id;
        if (vatWithNewCodeAlreadyExists)
        {
            throw new BusinessException(BookStoreDomainErrorCodes.VatWithCodeAlreadyExists).WithData("code", newCode);
        }

        vat.ChangeCode(newCode);
    }
}
