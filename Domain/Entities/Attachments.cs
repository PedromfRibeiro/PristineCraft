namespace PristineCraft.Domain.Entities;

public class Attachment(string fileName, string fileExtension, byte[] data)
{
    public string FileName { get; set; } = fileName;
    public string FileExtension { get; set; } = fileExtension;
    public byte[] Data { get; set; } = data;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
}