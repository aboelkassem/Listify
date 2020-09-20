using Listify.Domain.Lib.DTOs;
using Listify.Domain.Lib.Enums;

namespace Listify.Lib.DTOs
{
    public class PurchaseDTO: BaseDTO
    {
        public PurchaseMethod PurchaseMethod { get; set; }
        public float Subtotal { get; set; }
        public float AmountCharged { get; set; }
        public string PayerId { get; set; }
        public string OrderId { get; set; }
        public bool HasBeenCharged { get; set; }
        public bool WasChargeAccepted { get; set; }
    }
}
