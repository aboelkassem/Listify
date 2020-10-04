using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    public class PurchaseLineItemCurrency : PurchaseLineItem
    {
        public Guid ApplicationUserRoomCurrencyId { get; set; }
        public ApplicationUserRoomCurrencyRoom ApplicationUserRoomCurrency { get; set; }
    }
}
