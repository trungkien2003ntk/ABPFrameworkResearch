using Microsoft.AspNetCore.Mvc;
using MyDemo.BookStore.Vats;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;

namespace MyDemo.BookStore.Controllers;

[Route("/api/vats")]
public class VatController : ControllerBase
{
    private readonly VatAppService _vatAppService;

    public VatController(VatAppService vatAppService)
    {
        _vatAppService = vatAppService;
    }

    [HttpPost("import-from-excel")]
    public async Task<IActionResult> ImportExcel([FromForm] FileModel model)
    {
        if (model.File.FileName.IsNullOrWhiteSpace())
        {
            throw new UserFriendlyException("Invalid file.");
        }


        //using (var memoryStream = new MemoryStream())
        //{
        //    await model.File.GetStream().CopyToAsync(memoryStream);
        await _vatAppService.ImportFromExcelAsync(model.File);
        //}

        return NoContent();
    }

    [HttpGet("export-to-excel")]
    public async Task<IActionResult> ExportExcel()
    {
        var memoryStream = await _vatAppService.ExportToExcelAsync();

        // Return the file directly as a FileStreamResult
        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "VATs.xlsx");
    }
}

public class FileModel
{
    public IRemoteStreamContent File { get; set; }
}