using MyDemo.BookStore.Authors;
using MyDemo.BookStore.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Author = MyDemo.BookStore.Authors.Author;

namespace MyDemo.BookStore.Books;

[DependsOn(typeof(IExcelImporter))]
public class BooksAppService :
    CrudAppService<
        Book,
        BookDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateBookDto>,
    IBooksAppService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly AuthorManager _authorManager;
    private readonly BookManager _bookManager;
    private readonly IExcelImporter _importer;
    private readonly IExcelExporter _exporter;
    private readonly List<IExcelColumnDefinition> _columnDefinitions = [];


    public BooksAppService(
        IRepository<Book, Guid> repository,
        IAuthorRepository authorRepository,
        AuthorManager authorManager,
        BookManager bookManager,
        IExcelImporter importer,
        IExcelExporter exporter
    ) : base(repository)
    {
        _authorRepository = authorRepository;
        _authorManager = authorManager;
        _bookManager = bookManager;
        _importer = importer;
        _exporter = exporter;
        InitializeColumnDefinition();
    }

    public override async Task<BookDto> GetAsync(Guid id)
    {
        //Get the IQueryable<Book> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join books and authors
        var query = from book in queryable
                    join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
                    where book.Id == id
                    select new { book, author };

        //Execute the query and get the book with author
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Book), id);
        }

        var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
        bookDto.AuthorName = queryResult.author.Name;
        return bookDto;
    }

    public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Book> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join books and authors
        var query = from book in queryable
                    join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
                    select new { book, author };

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of BookDto objects
        var bookDtos = queryResult.Select(x =>
        {
            var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
            bookDto.AuthorName = x.author.Name;
            return bookDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<BookDto>(
            totalCount,
            bookDtos
        );
    }

    public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
    {
        var authors = await _authorRepository.GetListAsync();

        return new ListResultDto<AuthorLookupDto>(
            ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors)
        );
    }


    [RemoteService(IsEnabled = false)]
    public async Task<MemoryStream> GetBooksToExcelAsync()
    {
        var books = await Repository.GetListAsync();
        return await _exporter.ExportToStreamAsync(books, _columnDefinitions, "Books");
    }

    [RemoteService(IsEnabled = false)]
    public async Task ImportBooksFromExcelAsync(MemoryStream memoryStream)
    {
        _importer.SetColumnDefinition(_columnDefinitions);
        await _importer.ImportAsync<Book>(memoryStream);
    }

    private void InitializeColumnDefinition()
    {
        try
        {
            _columnDefinitions.AddRange([
                new ExcelColumnDefinition<string>("Name", false, value => value),
                new ExcelColumnDefinition<BookType>("Type", false, Enum.Parse<BookType>),
                new ExcelColumnDefinition<DateTime>("PublishDate", false, value => double.TryParse(value, out double parsedValue) ? DateTime.FromOADate(double.Parse(value)) : DateTime.ParseExact(value, "dd/MM/yyyy", null)),
                new ExcelColumnDefinition<float>("Price", false, float.Parse),
                new ExcelColumnDefinition<Guid>("AuthorId", false, Guid.Parse)
            ]);
        }
        catch (Exception ex)
        {
            throw new BusinessException(BookStoreDomainErrorCodes.InvalidValue).WithData("detail", ex.Message);
        }
    }

    private async Task<Author> CreateAuthorAsync(string authorName)
    {
        var author = await _authorManager.CreateAsync(authorName, default, default);
        await _authorRepository.InsertAsync(author);
        return author;
    }

    //private SheetData AppendHeaderRow(SheetData sheetData)
    //{
    //    var headerRow = new Row();
    //    headerRow.Append(
    //        CreateCell("Id", CellValues.String),
    //        CreateCell("Book Name", CellValues.String),
    //        CreateCell("Author Name", CellValues.String),
    //        CreateCell("Type", CellValues.String),
    //        CreateCell("Publish Date", CellValues.String),
    //        CreateCell("Price", CellValues.String));

    //    sheetData.AppendChild(headerRow);

    //    return sheetData;
    //}

    //private async Task<SheetData> AppendDataRows(IEnumerable<Book> books, SheetData sheetData)
    //{
    //    var authorIds = books.Select(b => b.AuthorId).ToList();
    //    var authors = await _authorRepository.GetListAsync(a => authorIds.Contains(a.Id));

    //    foreach (var book in books)
    //    {
    //        var author = authors.First(a => a.Id == book.AuthorId);
    //        var dataRow = new Row();
    //        dataRow.Append(
    //            CreateCell(book.Id.ToString(), CellValues.String),
    //            CreateCell(book.Name, CellValues.String),
    //            CreateCell(author.Name, CellValues.String),
    //            CreateCell(book.Type.ToString(), CellValues.String),
    //            CreateCell(book.PublishDate.ToString("yyyy-MM-dd"), CellValues.String), // Adjusted for consistent date format
    //            CreateCell(book.Price.ToString(), CellValues.Number));
    //        sheetData.AppendChild(dataRow);
    //    }

    //    return sheetData;
    //}

    
    private static string NormalizeSorting(string? sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"book.{nameof(Book.Name)}";
        }

        if (sorting.Contains("authorName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "authorName",
                "author.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"book.{sorting}";
    }
}
