using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.VMs
{
    public class CurrencyRoomVM : BaseVM
    {
        public string CurrencyName { get; set; } 
        public DateTime TimestampLastUpdate { get; set; } = DateTime.UtcNow;

        public RoomDTO Room { get; set; }
        public CurrencyDTO Currency { get; set; }

        public ICollection<ApplicationUserRoomCurrencyRoomDTO> ApplicationUsersRoomsCurrenciesRooms { get; set; } = // 3 references
            new List<ApplicationUserRoomCurrencyRoomDTO>();
    }
}
