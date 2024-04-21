using Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class ChatHub : Hub
{
    public async Task<string> SendMessage(string userNameFrom, string userNameTo, string messageContent)
    {
        //TODO: Null Check
        var chatService = Context?.GetHttpContext()?.RequestServices.GetService<ChatService>();
        if (chatService == null)
            return "Messenger_Error";
        return await chatService.SendMessage(userNameFrom, userNameTo, messageContent);
    }
}