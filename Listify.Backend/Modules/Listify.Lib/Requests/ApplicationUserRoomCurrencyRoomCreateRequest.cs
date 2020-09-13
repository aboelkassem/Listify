using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ApplicationUserRoomCurrencyRoomCreateRequest : BaseRequest
    {
        public int Quantity { get; set; }

        public Guid ApplicationUserRoomId { get; set; }
        public Guid CurrencyRoomId { get; set; }
    }
}
