using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDemo.BookStore.Books;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;

namespace MyDemo.BookStore.Controllers;

[Route("/api/books")]
public class BooksController : ControllerBase
{
    private BooksAppService _booksAppService;

    public BooksController(BooksAppService booksAppService)
    {
        _booksAppService = booksAppService;
    }

    [HttpPost("import-from-excel")]
    public async Task<IActionResult> ImportExcel([FromForm] FormModel model)
    {
        if (model.File == null || model.File.Length <= 0)
        {
            throw new UserFriendlyException("Invalid file.");
        }

        using (var memoryStream = new MemoryStream())
        {
            await model.File.CopyToAsync(memoryStream);
            await _booksAppService.ImportBooksFromExcelAsync(memoryStream);
        }

        return NoContent();
    }

    //[HttpGet("export-to-excel")]
    //public async Task<IActionResult> ExportExcel()
    //{
    //    var memoryStream = new MemoryStream();

    //    // Generate Excel file directly into the memory stream
    //    await _booksAppService.GetBooksToExcelAsync(memoryStream);

    //    // Reset the stream's position to the beginning
        //    memoryStream.Position = 0;

        //    // Return the file directly as a FileStreamResult
    //    return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Books.xlsx");
    //}
}

public class FormModel
{
    public IFormFile File { get; set; }
}
