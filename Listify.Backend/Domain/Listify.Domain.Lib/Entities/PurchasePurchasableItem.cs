using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("PurchasePurchasableItems", Schema = "Listify")]
    public class PurchasePurchasableItem : BaseEntity
    {
        public Guid PurchasableItemId { get; set; }
        public Guid PurchaseId { get; set; }

        public PurchasableItem PurchasableItem { get; set; }
        public Purchase Purchase { get; set; }
    }
}
