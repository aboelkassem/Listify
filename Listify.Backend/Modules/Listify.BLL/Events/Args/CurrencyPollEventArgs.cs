using Listify.Domain.BLL.Events.Args;
using Listify.Lib.DTOs;
using Listify.Lib.VMs;
using System.Collections.Generic;

namespace Listify.BLL.Events.Args
{
    public class CurrencyPollEventArgs : BasePollingEventArgs
    {
        public ICollection<ApplicationUserRoomCurrencyRoomVM> ApplicationUserRoomsCurrencies { get; set; } =
            new List<ApplicationUserRoomCurrencyRoomVM>();

        public RoomVM Room { get; set; }
    }
}
