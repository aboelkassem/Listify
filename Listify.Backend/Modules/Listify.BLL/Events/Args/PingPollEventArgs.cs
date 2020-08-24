using Listify.Domain.BLL.Events.Args;
using Listify.Lib.VMs;
using System.Collections.Generic;

namespace Listify.BLL.Events.Args
{
    public class PingPollEventArgs : BasePollingEventArgs
    {
        public ICollection<ApplicationUserRoomConnectionVM> ConnectionsPinged { get; set; } =
            new List<ApplicationUserRoomConnectionVM>();
    }
}
