using Application.Exception;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Services;

public class ChatService(IHubContext<ChatHub> _hubContext, DataContext _dbContext)
{
	public async Task<string> SendMessage(string userNameFrom, string userNameTo, string messageContent)
	{
		var sender = await _dbContext.DbUser.FirstOrDefaultAsync(u => u.Name == userNameFrom);
		var receiver = await _dbContext.DbUser.FirstOrDefaultAsync(u => u.Name == userNameTo);
		if (sender != null && receiver != null)
		{
			var message = new Message
			{
				Content = messageContent,
				Timestamp = DateTime.Now,
				Sender = sender,
				Receiver = receiver
			};

			_dbContext.DbMessages.Add(message);
			var response = await _dbContext.SaveChangesAsync();

			await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
			return response > 0 ? ExceptionStrings.Messenger_Sent : ExceptionStrings.Messenger_Error;
		}
		else
		{
			return ExceptionStrings.Messenger_Error;
		}
	}
}