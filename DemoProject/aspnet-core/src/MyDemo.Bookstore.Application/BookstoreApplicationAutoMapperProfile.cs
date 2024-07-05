using AutoMapper;
using MyDemo.Bookstore.Books;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyDemo.Bookstore;

public class BookstoreApplicationAutoMapperProfile : Profile
{
    public BookstoreApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
    }
}
