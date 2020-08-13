using Listify.Doimain.Lib.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("ApplicationUsersRooms", Schema = "Listify")]
    public class ApplicationUserRoom : BaseEntity
    {
        public Guid ApplciationUserId { get; set; }
        public Guid RoomId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Room Room { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; } =
            new List<ChatMessage>();

        public ICollection<ApplicationUserRoomCurrency> ApplicationUsersRoomsCurrencies { get; set; } =
            new List<ApplicationUserRoomCurrency>();

        public ICollection<ApplicationUserRoomConnection> ApplicationUsersRoomsConnections { get; set; } =
            new List<ApplicationUserRoomConnection>();
    }
}
