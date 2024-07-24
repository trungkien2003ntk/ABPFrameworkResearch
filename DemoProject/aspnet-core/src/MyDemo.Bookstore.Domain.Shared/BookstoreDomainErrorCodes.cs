namespace MyDemo.BookStore;

public static class BookStoreDomainErrorCodes
{
    public const string AuthorNotExistWithId = "BookStore.Error:100001";
    public const string AuthorNotExistWithName = "BookStore.Error:100002";
    public const string AuthorAlreadyExists = "BookStore.Error:100101";
    public const string BookAlreadyExists = "BookStore.Error:100102";
    public const string VatWithCodeAlreadyExists = "BookStore.Error:100103";

    public const string StoredProcedureError = "BookStore.Error:200001";
    public const string InvalidBookType = "BookStore.Error:400001";
    public const string InvalidPublishedDate = "BookStore.Error:400002";
    public const string InvalidPrice = "BookStore.Error:400003";
    public const string InvalidValue = "BookStore.Error:400004";
}
