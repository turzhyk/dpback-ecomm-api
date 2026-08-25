namespace DPBack.Infrastructure.Entities.Files;

public class FileEntity
{
    public Guid Id { get; set; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required long Size { get; set; }
    public required string StorageKey { get; set; }
    public DateTime CreatedAt { get; set; }
}