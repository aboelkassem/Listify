using Listify.Domain.Lib.DTOs;
using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.DTOs
{
    public class PurchasableItemDTO : BaseDTO
    {
        public string PurchasableItemName { get; set; }
        public PurchasableItemType PurchasableItemType { get; set; }
        public float Quantity { get; set; }
        public float UnitCost { get; set; }
        public string ImageUri { get; set; }
        public float DiscountApplied { get; set; }
    }
}
