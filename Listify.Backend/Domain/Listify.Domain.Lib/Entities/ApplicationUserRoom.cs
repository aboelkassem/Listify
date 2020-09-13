using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("ApplicationUsersRooms", Schema = "Listify")]
    public class ApplicationUserRoom : BaseEntity
    {
        public bool IsOnline { get; set; }
        public bool IsOwner { get; set; }

        public Guid ApplicationUserId { get; set; }
        public Guid RoomId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Room Room { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; } =
            new List<ChatMessage>();

        public ICollection<ApplicationUserRoomCurrencyRoom> ApplicationUsersRoomsCurrencies { get; set; } =
            new List<ApplicationUserRoomCurrencyRoom>();

        public ICollection<ApplicationUserRoomConnection> ApplicationUsersRoomsConnections { get; set; } =
            new List<ApplicationUserRoomConnection>();
    }
}
