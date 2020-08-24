using Listify.Domain.BLL.Events.Args;
using Listify.Lib.DTOs;
using Listify.Lib.VMs;
using System.Collections.Generic;

namespace Listify.BLL.Events.Args
{
    public class CurrencyPollEventArgs : BasePollingEventArgs
    {
        public ICollection<ApplicationUserRoomCurrencyVM> ApplicationUserRoomsCurrencies { get; set; } =
            new List<ApplicationUserRoomCurrencyVM>();

        public RoomDTO Room { get; set; }
    }
}
