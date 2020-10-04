using Listify.Domain.Lib.Requests;
using Listify.Lib.DTOs;
using Listify.Lib.VMs;

namespace Listify.Lib.Requests
{
    public class ApplicationUserUpdateRequest : BaseUpdateRequest
    {
        public string Username { get; set; }
        public string RoomCode { get; set; }
        public string RoomTitle { get; set; }
        public string RoomKey { get; set; }
        public bool AllowRequests { get; set; } = true;
        public bool IsRoomLocked { get; set; }
        public bool IsRoomPublic { get; set; } = true;
        public bool IsRoomOnline { get; set; } = true;
        public bool MatureContent { get; set; }
        public bool MatureContentChat { get; set; }
        public string ChatColor { get; set; }
        public string ProfileTitle { get; set; }
        public string ProfileDescription { get; set; }
        public RoomGenreVM[] RoomGenres { get; set; }
    }
}
