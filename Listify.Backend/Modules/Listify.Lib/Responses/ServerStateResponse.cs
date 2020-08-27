using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class ServerStateResponse : PlayFromServerResponse
    {
        public string ConnectionId { get; set; }
        public ServerStateType ServerStateType { get; set; }
        public PlayFromServerResponse SongCurrent { get; set; }
    }
}
