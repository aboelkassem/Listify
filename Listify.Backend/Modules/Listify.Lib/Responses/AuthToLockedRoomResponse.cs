using Listify.Domain.Lib.Enums;
using Listify.Lib.VMs;

namespace Listify.Lib.Responses
{
    public class AuthToLockedRoomResponse
    {
        public AuthToLockedRoomResponseType AuthToLockedRoomResponseType { get; set; }
        public RoomVM Room { get; set; }
    }
}
