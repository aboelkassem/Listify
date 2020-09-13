using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public class PurchaseLineItemVM : BaseVM
    {
        public PurchasableItemDTO PurchasableItem { get; set; }
        public PurchaseDTO Purchase { get; set; }
        public int orderQuantity { get; set; }
    }
}
