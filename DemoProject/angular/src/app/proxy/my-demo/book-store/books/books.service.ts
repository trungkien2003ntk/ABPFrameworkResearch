import type { AuthorLookupDto, BookDto, CreateUpdateBookDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { FileDto } from '../files/models';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  apiName = 'Default';


  create = (input: CreateUpdateBookDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'POST',
      url: '/api/app/books',
      body: input,
    },
      { apiName: this.apiName, ...config });


  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/books/${id}`,
    },
      { apiName: this.apiName, ...config });


  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'GET',
      url: `/api/app/books/${id}`,
    },
      { apiName: this.apiName, ...config });


  getAuthorLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<AuthorLookupDto>>({
      method: 'GET',
      url: '/api/app/books/author-lookup',
    },
      { apiName: this.apiName, ...config });


  getBooksToExcel = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileDto>({
      method: 'GET',
      url: '/api/app/books/books-to-excel',
    },
      { apiName: this.apiName, ...config });


  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<BookDto>>({
      method: 'GET',
      url: '/api/app/books',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
      { apiName: this.apiName, ...config });


  update = (id: string, input: CreateUpdateBookDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'PUT',
      url: `/api/app/books/${id}`,
      body: input,
    },
      { apiName: this.apiName, ...config });

  constructor(private restService: RestService) { }
}
