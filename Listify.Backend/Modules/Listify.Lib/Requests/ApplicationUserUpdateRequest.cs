using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ApplicationUserUpdateRequest : BaseUpdateRequest
    {
        public string Username { get; set; }
        public string RoomCode { get; set; }
        public string RoomTitle { get; set; }
        public string RoomKey { get; set; }
        public bool AllowRequests { get; set; }
        public bool IsRoomLocked { get; set; }
        public bool IsRoomPublic { get; set; }
        public bool IsRoomOnline { get; set; }
        public bool MatureContent { get; set; }
        public bool MatureContentChat { get; set; }
    }
}
