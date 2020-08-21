using Listify.Lib.DTOs;
using Listify.Lib.VMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.BLL.Args
{
    public class CurrencyPollEventArgs : BasePollingEventArgs
    {
        public ICollection<ApplicationUserRoomCurrencyVM> applicationUserRoomCurrencies { get; set; } =
            new List<ApplicationUserRoomCurrencyVM>();

        public RoomDTO Room { get; set; }
    }
}
