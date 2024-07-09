using Microsoft.AspNetCore.Authorization;
using MyDemo.BookStore.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MyDemo.BookStore.Authors;

public class AuthorAppService : BookStoreAppService, IAuthorAppService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly AuthorManager _authorManager;

    public AuthorAppService(
        IAuthorRepository authorRepository,
        AuthorManager authorManager)
    {
        _authorRepository = authorRepository;
        _authorManager = authorManager;
    }

    public async Task<AuthorDto> GetByNameAsync(string name)
    {
        var author = await _authorRepository.FindByNameAsync(name);

        return ObjectMapper.Map<Author, AuthorDto>(author);
    }

    public async Task TestStoredProcedureAsync(int value)
    {
        try
        {
            await _authorRepository.TestStoredProcedure(value);
        }
        catch (BusinessException ex)
        {
            throw new BusinessException(BookStoreDomainErrorCodes.StoredProcedureError, message: L[BookStoreDomainErrorCodes.StoredProcedureError])
                .WithData("{0}", ex.Message);
        }
    }

    public async Task<AuthorDto> GetAsync(Guid id)
    {
        var author = await _authorRepository.GetAsync(id);
        return ObjectMapper.Map<Author, AuthorDto>(author);
    }

    public async Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Author.Name);
        }

        var authors = await _authorRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
        ? await _authorRepository.CountAsync()
        : await _authorRepository.CountAsync(
            author => author.Name.Contains(input.Filter));

        return new PagedResultDto<AuthorDto>(
            totalCount,
            ObjectMapper.Map<List<Author>, List<AuthorDto>>(authors)
        );
    }

    [Authorize(Policy = BookStorePermissions.Authors.Create)]
    public async Task<AuthorDto> CreateAsync(CreateAuthorDto input)
    {
        var newAuthor = await _authorManager.CreateAsync(
            input.Name,
            input.BirthDate,
            input.ShortBio
        );

        await _authorRepository.InsertAsync(newAuthor);

        return ObjectMapper.Map<Author, AuthorDto>(newAuthor);
    }

    [Authorize(Policy = BookStorePermissions.Authors.Edit)]
    public async Task UpdateAsync(Guid id, UpdateAuthorDto input)
    {
        var newName = input.Name;
        var existingAuthor = await _authorRepository.GetAsync(id);

        if (existingAuthor.Name != newName)
        {
            await _authorManager.ChangeNameAsync(existingAuthor, newName);
        }

        existingAuthor.BirthDate = input.BirthDate;
        existingAuthor.ShortBio = input.ShortBio;

        await _authorRepository.UpdateAsync(existingAuthor);
    }

    [Authorize(Policy = BookStorePermissions.Authors.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _authorRepository.DeleteAsync(id);
    }
}
