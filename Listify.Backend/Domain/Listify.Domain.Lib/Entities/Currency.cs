using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    // Currency is controlled by the product owner and can be earned
    // by participating in channels over time or purchased.
    // Can do specials / group event awards
    [Table("Currencies", Schema = "Listify")]
    public class Currency : BaseEntity
    {
        public string CurrencyName { get; set; }
        public int Weight { get; set; }
        public int QuantityIncreasePerTick { get; set; }
        public int TimeSecBetweenTick { get; set; }
        public DateTime TimestampLastUpdated { get; set; } = DateTime.UtcNow;

        public ICollection<ApplicationUserRoomCurrency> ApplicationUsersRoomsCurrencies { get; set; } =
            new List<ApplicationUserRoomCurrency>();
    }
}
