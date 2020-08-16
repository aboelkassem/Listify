using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public class ApplicationUserRoomConnectionVM : BaseVM
    {
        // This is assigned for SignalR
        public string ConnectionId { get; set; }

        public bool IsOnline { get; set; }
        public bool HasPingBeenSent { get; set; }

        public ApplicationUserRoomDTO ApplicationUserRoom { get; set; }
    }
}
