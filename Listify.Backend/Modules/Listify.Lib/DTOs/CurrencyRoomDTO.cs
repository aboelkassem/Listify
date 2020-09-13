using Listify.Domain.Lib.DTOs;
using System;
using System.Collections.Generic;

namespace Listify.Lib.DTOs
{
    public class CurrencyRoomDTO : BaseDTO
    {
        public string CurrencyName { get; set; }
        public DateTime TimestampLastUpdate { get; set; } = DateTime.UtcNow;
    }
}
