using Listify.Domain.Lib.DTOs;
using Listify.Domain.Lib.Entities;
using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.DTOs
{
    public class PurchaseLineItemDTO : BaseDTO
    {
        public int orderQuantity { get; set; }

        public Guid PurchasableItemId { get; set; }
        public Guid PurchaseId { get; set; }
    }
}
