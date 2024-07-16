import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BooksService, BookDto, bookTypeOptions, AuthorLookupDto } from '@proxy/my-demo/book-store/books';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { FileDto } from '@proxy/my-demo/book-store/files';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss'],
  providers: [
    ListService,
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }
  ],
})
export class BookComponent implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  form: FormGroup;

  selectedBook = {} as BookDto;

  authors$: Observable<AuthorLookupDto[]>;

  bookTypes = bookTypeOptions;

  isModalOpen = false;

  constructor(
    public readonly list: ListService,
    private booksService: BooksService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {
    this.authors$ = booksService.getAuthorLookup().pipe(map((r) => r.items));
  }

  ngOnInit() {
    const bookStreamCreator = (query) => this.booksService.getList(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
  }

  createBook() {
    this.selectedBook = {} as BookDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editBook(id: string) {
    this.booksService.get(id).subscribe((book) => {
      this.selectedBook = book;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  onChange(authorId: string) {
    console.log("Selected author ID:", authorId);
  }

  buildForm() {
    this.form = this.fb.group({
      authorId: [this.selectedBook.authorId || null, Validators.required],
      name: [this.selectedBook.name || null, Validators.required],
      type: [this.selectedBook.type || null, Validators.required],
      publishDate: [
        this.selectedBook.publishDate ? new Date(this.selectedBook.publishDate) : null,
        Validators.required,
      ],
      price: [this.selectedBook.price || null, Validators.required],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedBook.id
      ? this.booksService.update(this.selectedBook.id, this.form.value)
      : this.booksService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.booksService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  exportToExcel() {
    this.booksService.getBooksToExcel().subscribe((response: FileDto) => {
      const byteCharacters = atob(response.fileBytes);
      const byteNumbers = new Uint8Array(byteCharacters.length);
      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      const blob = new Blob([byteNumbers], {
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
      });
      FileSaver.saveAs(blob, response.downloadName);
    });
  }
}
