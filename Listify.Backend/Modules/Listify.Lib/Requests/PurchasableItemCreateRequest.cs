using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.Requests;
using System;

namespace Listify.Lib.Requests
{
    public class PurchasableItemCreateRequest : BaseRequest
    {
        public Guid Id { get; set; }
        public string PurchasableItemName { get; set; }
        public PurchasableItemType PurchasableItemType { get; set; }
        public float Quantity { get; set; }
        public float UnitCost { get; set; }
        public string ImageUri { get; set; }
    }
}
