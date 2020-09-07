using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    public class Purchase : BaseEntity
    {
        public PurchaseMethod PurchaseMethod { get; set; }
        public float Subtotal { get; set; }
        public float AmountCharged { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<PurchasePurchasableItem> PurchasePurchasableItems { get; set; } =
            new List<PurchasePurchasableItem>();
    }
}
