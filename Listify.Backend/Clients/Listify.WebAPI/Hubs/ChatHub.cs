using Listify.Doimain.Lib.Entities;
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
        public async Task SendMessage(ChatMessage message)
        {
            await Clients.All.SendAsync("RecieveMessage", message);
        }

        public async Task RequestSong(SongQueuedCreateRequest request)
        {

        }
    }
}
