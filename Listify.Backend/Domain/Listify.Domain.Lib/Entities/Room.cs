using Listify.Doimain.Lib.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("Rooms", Schema = "Listify")]
    public class Room : BaseEntity
    {
        // the identifier for the room that can be chosen by the user
        public string RoomCode { get; set; }
        public bool IsRoomPublic { get; set; }
        public bool IsRoomOnline { get; set; }

        // This is the Room Owner
        public Guid ApplicationUserId { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<ApplicationUserRoom> ApplicationUsersRooms { get; set; } =
            new List<ApplicationUserRoom>();
        
        // this is SongQueued instead of song Request because if isn't
        // a song in the queue, a record will be created for one for the
        // SongRequest in the playlist.
        public ICollection<SongQueued> SongsQueued { get; set; } =
            new List<SongQueued>();

        public ICollection<Currency> Currencies { get; set; } =
            new List<Currency>();
    }
}
