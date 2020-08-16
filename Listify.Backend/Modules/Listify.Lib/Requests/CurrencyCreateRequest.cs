using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class CurrencyCreateRequest : BaseRequest
    {
        public string CurrencyName { get; set; }
        public int Weight { get; set; }
        public int QuantityIncreasePerTick { get; set; }
        public int TimeSecBetweenTick { get; set; }

        public Guid RoomId { get; set; }
    }
}
