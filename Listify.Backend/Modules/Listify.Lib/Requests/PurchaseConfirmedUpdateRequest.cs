using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.Requests;
using Listify.Lib.VMs;
using System;

namespace Listify.Lib.Requests
{
    public class PurchaseConfirmedUpdateRequest
    {
        public PurchaseVM Purchase { get; set; }
        public bool WasChargeAccepted { get; set; }
    }
}
