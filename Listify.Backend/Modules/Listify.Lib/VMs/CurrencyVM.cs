using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Lib.VMs
{
    // Currency is controlled by the product owner and can be earned
    // by participating in channels over time or purchased.
    // Can do specials / group event awards
    public class CurrencyVM : BaseVM
    {
        public string CurrencyName { get; set; }
        public int Weight { get; set; }
        public int QuantityIncreasePerTick { get; set; }
        public int TimeSecBetweenTick { get; set; }
        public DateTime TimestampLastUpdated { get; set; } = DateTime.UtcNow;

        public RoomDTO Room { get; set; }

        public ICollection<ApplicationUserRoomCurrencyDTO> ApplicationUsersRoomsCurrencies { get; set; } =
            new List<ApplicationUserRoomCurrencyDTO>();
    }
}
