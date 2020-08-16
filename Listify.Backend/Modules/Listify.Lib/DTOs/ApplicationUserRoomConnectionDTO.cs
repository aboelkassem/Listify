using Listify.Domain.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Lib.DTOs
{
    public class ApplicationUserRoomConnectionDTO : BaseDTO
    {
        // This is assigned for SignalR
        public string ConnectionId { get; set; }

        public bool IsOnline { get; set; }
        public bool HasPingBeenSent { get; set; }
    }
}
