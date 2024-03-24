namespace Domain.Entities.Services;

public class EmailMessageModel
{
	public string ToAddress { get; set; }
	public string Subject { get; set; }
	public string? Body { get; set; }
	public byte[]? AttachmentPath { get; set; }

	public EmailMessageModel(string toAddress, string subject, string? body = "", byte[] attachmentPath = null)
	{
		ToAddress = toAddress;
		Subject = subject;
		Body = body;
		AttachmentPath = attachmentPath;
	}
}