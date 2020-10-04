using Listify.Domain.Lib.DTOs;

namespace Listify.Lib.DTOs
{
    public class RoomDTO : BaseDTO
    {
        // the identifier for the room that can be chosen by the user
        public string RoomCode { get; set; }
        public string RoomTitle { get; set; }
        public bool AllowRequests { get; set; }
        public bool IsRoomLocked { get; set; }
        public bool IsRoomPublic { get; set; } = true;
        public bool IsRoomOnline { get; set; } = true;
        public bool IsRoomPlaying { get; set; }
        public bool MatureContent { get; set; }
        public bool MatureContentChat { get; set; }
        public int NumberUsersOnline { get; set; }
        public string RoomImageUrl { get; set; }
        public int NumberFollows { get; set; }
    }
}
