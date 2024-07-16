namespace MyDemo.BookStore.Files;

public class FileDto
{
    public string FileBytes { get; set; }
    public string DownloadName { get; set; }

    public FileDto(string downloadName, string fileBytes)
    {
        DownloadName = downloadName;
        FileBytes = fileBytes;
    }
}
