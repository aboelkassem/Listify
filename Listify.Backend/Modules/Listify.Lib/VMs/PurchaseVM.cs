using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class PurchaseVM : BaseVM
    {
        public PurchaseMethod PurchaseMethod { get; set; }
        public float Subtotal { get; set; }
        public float AmountCharged { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }

        public ICollection<PurchaseLineItemVM> PurchaseLineItems { get; set; } =
            new List<PurchaseLineItemVM>();
    }
}
