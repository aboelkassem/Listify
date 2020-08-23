using Listify.Domain.BLL.Args;
using Listify.Lib.VMs;
using System.Collections.Generic;

namespace Listify.BLL.Args
{
    public class PingPollEventArgs : BasePollingEventArgs
    {
        public ICollection<ApplicationUserRoomConnectionVM> ConnectionsPinged { get; set; } =
            new List<ApplicationUserRoomConnectionVM>();
        public ICollection<ApplicationUserRoomConnectionVM> ConnectionsRemoved { get; set; } =
            new List<ApplicationUserRoomConnectionVM>();
    }
}
