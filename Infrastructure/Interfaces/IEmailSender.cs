﻿using Domain.Entities.Services;

namespace Infrastructure.Interfaces;

public interface IEmailSender
{
	Task SendEmailAsync(string email, string subject, string htmlMessage, byte[]? attachment);
}