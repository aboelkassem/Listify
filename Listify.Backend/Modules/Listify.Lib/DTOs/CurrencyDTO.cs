using Listify.Domain.Lib.DTOs;
using System;

namespace Listify.Lib.DTOs
{
    // Currency is controlled by the product owner and can be earned
    // by participating in channels over time or purchased.
    // Can do specials / group event awards
    public class CurrencyDTO : BaseDTO
    {
        public string CurrencyName { get; set; }
        public int Weight { get; set; }
        public int QuantityIncreasePerTick { get; set; }
        public int TimeSecBetweenTick { get; set; }
    }
}
