using Listify.Domain.Lib.Entities;
using Listify.Lib.Requests;

namespace Listify.Lib.Requests
{
    public class PaypalPaymentRequest
    {
        public string PayerID { get; set; }
        public string OrderID { get; set; }
        public PurchaseCreateRequest Order { get; set; }
        public PaypalPaymentCreateRequest Payment { get; set; }
    }
}
