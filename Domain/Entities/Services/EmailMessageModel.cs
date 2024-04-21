namespace PristineCraft.Domain.Entities.Services;

public class EmailMessageModel(string toAddress, string subject, string? body = "", byte[]? attachmentPath = null)
{
    public string ToAddress { get; set; } = toAddress;
    public string Subject { get; set; } = subject;
    public string? Body { get; set; } = body;
    public byte[]? AttachmentPath { get; set; } = attachmentPath;
}