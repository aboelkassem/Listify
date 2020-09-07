using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class PurchasableItemVM : BaseVM
    {
        public string PurchasableItemName { get; set; }
        public PurchasableItemType PurchasableItemType { get; set; }
        public float Quantity { get; set; }
        public float UnitCost { get; set; }
        public string ImageUri { get; set; }
        public float DiscountApplied { get; set; }

        public ICollection<PurchasePurchasableItemDTO> PurchasePurchasableItems { get; set; } =
            new List<PurchasePurchasableItemDTO>();
    }
}
