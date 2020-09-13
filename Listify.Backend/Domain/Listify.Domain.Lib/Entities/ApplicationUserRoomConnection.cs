using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("ApplicationUsersRoomsConnections", Schema = "Listify")]
    public class ApplicationUserRoomConnection : BaseEntity
    {
        // This is assigned for SignalR
        public string ConnectionId { get; set; }

        public bool IsOnline { get; set; }
        public bool HasPingBeenSent { get; set; }
        public ConnectionType ConnectionType { get; set; }

        public Guid ApplicationUserRoomId { get; set; }
        public ApplicationUserRoom ApplicationUserRoom { get; set; }
    }
}
