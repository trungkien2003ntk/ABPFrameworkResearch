using AutoMapper;
using MyDemo.BookStore.Authors;
using MyDemo.BookStore.Books;
using MyDemo.BookStore.Categories;
using MyDemo.BookStore.SystemCategories;
using MyDemo.BookStore.Vats;

namespace MyDemo.BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<Author, AuthorDto>();
        CreateMap<Author, AuthorLookupDto>();
        CreateMap<Vat, VatDto>();
        CreateMap<SystemCategory, SystemCategoryDto>();
    }
}
