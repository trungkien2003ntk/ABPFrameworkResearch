using AutoMapper;
using MyDemo.BookStore.Authors;
using MyDemo.BookStore.Books;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyDemo.BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<Author, AuthorDto>();
        CreateMap<Author, AuthorLookupDto>();
    }
}
