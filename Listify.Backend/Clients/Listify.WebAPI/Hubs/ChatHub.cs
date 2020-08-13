using Listify.WebAPI.Models.Entities;
using Listify.WebAPI.Models.Requests;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.WebAPI.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("RecieveMessage", message);
        }

        public async Task RequestSong(SongQueuedCreateRequest request)
        {

        }
    }
}
