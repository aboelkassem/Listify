using Listify.Domain.Lib.Requests;
using System;

namespace Listify.Lib.Requests
{
    public class ApplicationUserRoomConnectionUpdateRequest : BaseUpdateRequest
    {
        public Guid ApplicationUserRoomId { get; set; }
        public bool IsOnline { get; set; }
        public bool HasPingBeenSent { get; set; }
    }
}
