using Listify.Domain.Lib.Requests;

namespace Listify.Lib.Requests
{
    public class RoomUpdateRequest : BaseUpdateRequest
    {
        // the identifier for the room that can be chosen by the user
        public string RoomCode { get; set; }
        public string RoomTitle { get; set; }
        public string RoomKey { get; set; }
        public bool AllowRequests { get; set; } = true;
        public bool IsRoomLocked { get; set; }
        public bool IsRoomPublic { get; set; } = true;
        public bool IsRoomOnline { get; set; } = true;
        public int NumberUsersOnline { get; set; }
    }
}
