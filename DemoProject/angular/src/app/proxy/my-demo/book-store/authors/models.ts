import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { BookDto } from '../books/models';

export interface AuthorDto extends EntityDto<string> {
  name?: string;
  birthDate?: string;
  shortBio?: string;
  books: BookDto[];
}

export interface CreateAuthorDto {
  name: string;
  birthDate: string;
  shortBio?: string;
}

export interface GetAuthorListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface UpdateAuthorDto {
  name: string;
  birthDate: string;
  shortBio?: string;
}
