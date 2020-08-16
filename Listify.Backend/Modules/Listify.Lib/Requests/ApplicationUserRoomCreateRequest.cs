using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ApplicationUserRoomCreateRequest : BaseRequest
    {
        public bool IsOnline { get; set; }
        public Guid RoomId { get; set; }
    }
}
