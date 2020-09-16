using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("Rooms", Schema = "Listify")]
    public class Room : BaseEntity
    {
        // the identifier for the room that can be chosen by the user
        public string RoomCode { get; set; }
        public string RoomTitle { get; set; }
        public string RoomKey { get; set; }
        public bool AllowRequests { get; set; } = true;
        public bool IsRoomLocked { get; set; }
        public bool IsRoomPublic { get; set; } = true;
        public bool IsRoomOnline { get; set; } = true;
        public bool MatureContent { get; set; }
        public bool MatureContentChat { get; set; }

        // This is the Room Owner
        public Guid ApplicationUserId { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<ApplicationUserRoom> ApplicationUsersRooms { get; set; } =
            new List<ApplicationUserRoom>();

        public ICollection<CurrencyRoom> CurrenciesRoom { get; set; } =
            new List<CurrencyRoom>();

        // this is SongQueued instead of song Request because if isn't
        // a song in the queue, a record will be created for one for the
        // SongRequest in the playlist.
        public ICollection<SongQueued> SongsQueued { get; set; } =
            new List<SongQueued>();
    }
}
