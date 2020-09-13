using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("PurchaseLineItems", Schema = "Listify")]
    public class PurchaseLineItem : BaseEntity
    {
        public int OrderQuantity { get; set; }

        public Guid PurchasableItemId { get; set; }
        public Guid PurchaseId { get; set; }

        public PurchasableItem PurchasableItem { get; set; }
        public Purchase Purchase { get; set; }
    }
}
