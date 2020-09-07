using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public class PurchasePurchasableItemVM : BaseVM
    {
        public PurchasableItemDTO PurchasableItem { get; set; }
        public PurchaseDTO Purchase { get; set; }
    }
}
