using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class RoomVM : BaseVM
    {
        // the identifier for the room that can be chosen by the user
        public string RoomCode { get; set; }
        public string RoomTitle { get; set; }
        public string RoomKey { get; set; }
        public bool AllowRequests { get; set; }
        public bool IsRoomLocked { get; set; }
        public bool IsRoomPublic { get; set; } = true;
        public bool IsRoomOnline { get; set; } = true;
        public bool IsRoomPlaying { get; set; }
        public int NumberUsersOnline { get; set; }
        public bool MatureContent { get; set; }
        public bool MatureContentChat { get; set; }
        public string RoomImageUrl { get; set; }

        // This is the Room Owner
        public ApplicationUserDTO ApplicationUser { get; set; }

        public ICollection<ApplicationUserRoomDTO> ApplicationUsersRooms { get; set; } =
            new List<ApplicationUserRoomDTO>();

        // this is SongQueued instead of song Request because if isn't
        // a song in the queue, a record will be created for one for the
        // SongRequest in the playlist.
        public ICollection<SongQueuedDTO> SongsQueued { get; set; } =
            new List<SongQueuedDTO>();

        public ICollection<FollowVM> Follows { get; set; } =
            new List<FollowVM>();

        public ICollection<RoomGenreVM> RoomGenres { get; set; } =
            new List<RoomGenreVM>();
    }
}
