using Listify.Domain.Lib.DTOs;
using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.DTOs
{
    public class PurchaseLineItemCurrencyDTO : BaseDTO
    {
        public Guid ApplicationUserRoomCurrencyId { get; set; }
    }
}
