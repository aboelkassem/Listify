using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    public class PurchasePurchasableItem : BaseEntity
    {
        public Guid PurchasableItemId { get; set; }
        public Guid PurchaseId { get; set; }

        public PurchasableItem PurchasableItem { get; set; }
        public Purchase Purchase { get; set; }
    }
}
