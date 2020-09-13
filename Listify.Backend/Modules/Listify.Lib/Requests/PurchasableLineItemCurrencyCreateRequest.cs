using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class PurchasableLineItemCurrencyCreateRequest: PurchasableLineItemCreateRequest
    {
        public Guid ApplicationUserRoomCurrencyId { get; set; }
    }
}
