using Domain.Entities.Services;
using FluentEmail.Core.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Controllers;

namespace ServiceApi.Controllers;

public class EmailController : BaseApiController
{
	private readonly IEmailService _emailService;

	public EmailController(IEmailService emailService)
	{
		_emailService = emailService
			?? throw new ArgumentNullException(nameof(emailService));
	}

	[HttpGet("singleemail")]
	public async Task<IActionResult> SendSingleEmail()
	{
		EmailMessageModel emailMetadata = new("john.doe@gmail.com", "FluentEmail Test email", "This is a test email from FluentEmail.");
		await _emailService.Send(emailMetadata);
		return Ok();
	}
}