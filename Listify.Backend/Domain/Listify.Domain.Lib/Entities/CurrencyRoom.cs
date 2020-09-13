using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("CurrenciesRoom", Schema = "Listify")]
    public class CurrencyRoom : BaseEntity  // 11 references
    {
        public Guid RoomId { get; set; }    // 4 references
        public Guid CurrencyId { get; set; } // 3 references

        public string CurrencyName { get; set; } // 2 references
        public DateTime TimestampLastUpdate { get; set; } = DateTime.UtcNow; // 1 references

        public Room Room { get; set; } // 3 references
        public Currency Currency { get; set; } // 3 references

        public ICollection<ApplicationUserRoomCurrencyRoom> ApplicationUsersRoomsCurrenciesRooms { get; set; } = // 3 references
            new List<ApplicationUserRoomCurrencyRoom>();
    }
}
