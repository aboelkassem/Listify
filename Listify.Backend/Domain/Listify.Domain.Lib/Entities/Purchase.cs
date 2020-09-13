using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("Purchases", Schema = "Listify")]
    public class Purchase : BaseEntity
    {
        public PurchaseMethod PurchaseMethod { get; set; }
        public float Subtotal { get; set; }
        public float AmountCharged { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<PurchaseLineItem> PurchaseLineItems { get; set; } =
            new List<PurchaseLineItem>();
    }
}
