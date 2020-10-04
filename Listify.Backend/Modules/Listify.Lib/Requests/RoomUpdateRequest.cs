using Listify.Domain.Lib.Requests;
using Listify.Lib.VMs;

namespace Listify.Lib.Requests
{
    public class RoomUpdateRequest : BaseUpdateRequest
    {
        public bool IsRoomOnline { get; set; }
        public bool IsRoomPlaying { get; set; }

        public RoomGenreVM[] RoomGenres { get; set; }

        //// the identifier for the room that can be chosen by the user
        //public string RoomCode { get; set; }
        //public string RoomTitle { get; set; }
        //public string RoomKey { get; set; }
        //public bool AllowRequests { get; set; } = true;
        //public bool IsRoomLocked { get; set; }
        //public bool IsRoomPublic { get; set; } = true;
        //public bool IsRoomOnline { get; set; } = true;
        //public bool IsRoomPlaying { get; set; }
        //public int NumberUsersOnline { get; set; }
        //public bool MatureContentChat { get; set; }
        //public bool MatureContent { get; set; }
    }
}
