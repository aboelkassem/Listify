using Listify.Domain.Lib.Requests;

namespace Listify.Lib.Requests
{
    public class RoomUpdateRequest : BaseUpdateRequest
    {
        // the identifier for the room that can be chosen by the user
        public string RoomCode { get; set; }
        public bool IsRoomPublic { get; set; }
        public bool IsRoomOnline { get; set; }
    }
}
