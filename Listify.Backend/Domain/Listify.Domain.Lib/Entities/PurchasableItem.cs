using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    public class PurchasableItem : BaseEntity
    {
        public string PurchasableItemName { get; set; }
        public PurchasableItemType PurchasableItemType { get; set; }
        public float Quantity { get; set; }
        public float UnitCost { get; set; }
        public string ImageUri { get; set; }
        public float DiscountApplied { get; set; }

        public ICollection<PurchasePurchasableItem> PurchasePurchasableItems { get; set; } =
            new List<PurchasePurchasableItem>();
    }
}
