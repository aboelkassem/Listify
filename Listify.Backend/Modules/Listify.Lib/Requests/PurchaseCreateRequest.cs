using Listify.Domain.Lib.Enums;

namespace Listify.Lib.Requests
{
    public class PurchaseCreateRequest
    {
        public PurchaseMethod PurchaseMethod { get; set; }
        public float Subtotal { get; set; }
        public float AmountCharged { get; set; }
        public string PayerId { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string[] PurchasableItemsJSON { get; set; }
    }
}
