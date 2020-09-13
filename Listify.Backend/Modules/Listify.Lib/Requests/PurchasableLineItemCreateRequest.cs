using Listify.Domain.Lib.Entities;
using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class PurchasableLineItemCreateRequest: BaseRequest
    {
        public PurchasableItem PurchasableItem { get; set; }
        public int OrderQuantity { get; set; }
    }
}
