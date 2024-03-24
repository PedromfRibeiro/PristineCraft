using Application.Exception;
using Domain.Entities.User;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Persistence;

namespace Infrastructure.Services;

public class ChatService(IHubContext<ChatHub> _hubContext, DataContext _dbContext)
{
	public async Task<string> SendMessage(string userNameFrom, string userNameTo, string messageContent)
	{
		var userFrom = await _dbContext.db_User.FirstOrDefaultAsync(u => u.Name == userNameFrom);
		var userTo = await _dbContext.db_User.FirstOrDefaultAsync(u => u.Name == userNameTo);
		if (userFrom != null && userTo != null)
		{
			var message = new Message
			{
				Content = messageContent,
				Timestamp = DateTime.Now,
				UserFrom = userFrom,
				UserTo = userTo
			};

			_dbContext.db_Messages.Add(message);
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