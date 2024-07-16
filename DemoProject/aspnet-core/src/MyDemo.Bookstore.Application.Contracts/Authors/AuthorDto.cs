using MyDemo.BookStore.Books;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace MyDemo.BookStore.Authors;

public class AuthorDto : EntityDto<Guid>
{
    public string Name { get; set; }

    public DateTime BirthDate { get; set; }

    public string ShortBio { get; set; }

    public List<BookDto> Books { get; set; }
}