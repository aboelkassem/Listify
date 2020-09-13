using Listify.Domain.Lib.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("PurchasableItems", Schema = "Listify")]
    public class PurchasableItem : BaseEntity
    {
        public string PurchasableItemName { get; set; }
        public PurchasableItemType PurchasableItemType { get; set; }
        public int Quantity { get; set; }
        public float UnitCost { get; set; }
        public string ImageUri { get; set; }
        public float DiscountApplied { get; set; }
    }
}
