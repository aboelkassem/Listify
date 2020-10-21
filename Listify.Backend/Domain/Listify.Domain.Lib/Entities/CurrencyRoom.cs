using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("CurrenciesRoom", Schema = "Listify")]
    public class CurrencyRoom : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid CurrencyId { get; set; }

        public string CurrencyName { get; set; }
        public DateTime TimestampLastUpdate { get; set; } = DateTime.UtcNow;

        public Room Room { get; set; }
        public Currency Currency { get; set; }

        public ICollection<ApplicationUserRoomCurrencyRoom> ApplicationUsersRoomsCurrenciesRooms { get; set; } =
            new List<ApplicationUserRoomCurrencyRoom>();
    }
}
