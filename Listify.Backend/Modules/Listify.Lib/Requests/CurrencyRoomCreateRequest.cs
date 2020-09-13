using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class CurrencyRoomCreateRequest: BaseRequest
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid CurrencyId { get; set; }

        public string CurrencyName { get; set; }
        public DateTime TimestampLastUpdate { get; set; } = DateTime.UtcNow;
    }
}
