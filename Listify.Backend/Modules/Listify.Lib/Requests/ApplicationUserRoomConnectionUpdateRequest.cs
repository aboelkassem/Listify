using Listify.Domain.Lib.Requests;

namespace Listify.Lib.Requests
{
    public class ApplicationUserRoomConnectionUpdateRequest : BaseUpdateRequest
    {
        public bool IsOnline { get; set; }
        public bool HasPingBeenSent { get; set; }
    }
}
