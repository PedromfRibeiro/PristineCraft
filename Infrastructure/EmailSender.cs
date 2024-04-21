using PristineCraft.Domain.Entities.Services;
using Infrastructure.Interfaces;

namespace Infrastructure;

public class EmailSender : IEmailSender
{
    private readonly IEmailService _emailService;

    public EmailSender(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage, byte[]? attachment = null)
    {
        EmailMessageModel emailMessage = new EmailMessageModel(email, subject, htmlMessage, attachment);
        await _emailService.Send(emailMessage);
    }
}