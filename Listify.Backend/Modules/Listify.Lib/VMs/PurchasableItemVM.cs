using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.VMs;

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
    }
}
