using Listify.Domain.Lib.Entities;
using Listify.WebAPI.Models.Requests;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Listify.WebAPI.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(ChatMessage message)
        {
            await Clients.All.SendAsync("RecieveMessage", message);
        }

        public async Task RequestSong(SongQueuedCreateRequest request)
        {

        }
    }
}
