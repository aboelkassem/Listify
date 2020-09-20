using Listify.Lib.Requests;
using Listify.Lib.Responses;
using System.Threading.Tasks;

namespace Listify.Services
{
    public interface IListifyService
    {
        Task<string> CleanContent(string content);
        Task<bool> IsContentValid(string content);

        Task<bool> ExcecutePaypalTransaction(PurchaseCreateRequest purchase);
        Task<PaypalPaymentCreateResponse> CreatePaypalPayment(PaypalPaymentCreateRequest request);
    }
}