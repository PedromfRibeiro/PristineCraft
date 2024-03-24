using Domain.Entities.User;
using Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class ChatHub : Hub
{
	public async Task<string> SendMessage(string userNameFrom, string userNameTo, string messageContent)
	{
		var chatService = Context.GetHttpContext().RequestServices.GetService<ChatService>();
		return await chatService.SendMessage(userNameFrom, userNameTo, messageContent);
	}
}