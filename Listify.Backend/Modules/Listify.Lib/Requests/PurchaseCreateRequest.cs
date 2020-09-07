using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.Requests;
using System;

namespace Listify.Lib.Requests
{
    public class PurchaseCreateRequest
    {
        public PurchaseMethod PurchaseMethod { get; set; }
        public float Subtotal { get; set; }
        public float AmountCharged { get; set; }

        public Guid[] PurchasableItemsIds { get; set; }
    }
}
