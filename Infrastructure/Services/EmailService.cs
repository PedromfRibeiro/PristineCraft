using PristineCraft.Domain.Entities.Services;
using FluentEmail.Core;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class EmailService(ILogger<EmailService> _logger, IFluentEmail _fluentEmail) : IEmailService
{
	public async Task Send(EmailMessageModel emailMessageModel)
	{
		_logger.LogInformation("Sending email");
		await _fluentEmail
			.To(emailMessageModel.ToAddress)
			.Subject(emailMessageModel.Subject)
			.Body(emailMessageModel.Body, true) // true means this is an HTML format message
			.SendAsync();
	}
}