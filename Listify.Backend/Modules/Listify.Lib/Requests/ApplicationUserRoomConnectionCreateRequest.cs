using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ApplicationUserRoomConnectionCreateRequest : BaseRequest
    {
        // This is assigned for SignalR
        public string ConnectionId { get; set; }

        public bool IsOnline { get; set; }
        public bool HasPingBeenSent { get; set; }

        public Guid ApplicationUserRoomId { get; set; }
    }
}
