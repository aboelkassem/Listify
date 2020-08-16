using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class RoomVM : BaseVM
    {
        // the identifier for the room that can be chosen by the user
        public string RoomCode { get; set; }
        public bool IsRoomPublic { get; set; }
        public bool IsRoomOnline { get; set; }

        // This is the Room Owner
        public ApplicationUserDTO ApplicationUser { get; set; }

        public ICollection<ApplicationUserRoomDTO> ApplicationUsersRooms { get; set; } =
            new List<ApplicationUserRoomDTO>();
        
        // this is SongQueued instead of song Request because if isn't
        // a song in the queue, a record will be created for one for the
        // SongRequest in the playlist.
        public ICollection<SongQueuedDTO> SongsQueued { get; set; } =
            new List<SongQueuedDTO>();

        public ICollection<CurrencyDTO> Currencies { get; set; } =
            new List<CurrencyDTO>();
    }
}
