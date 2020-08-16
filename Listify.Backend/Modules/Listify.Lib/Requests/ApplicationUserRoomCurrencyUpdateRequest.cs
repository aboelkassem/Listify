using Listify.Domain.Lib.Requests;

namespace Listify.Lib.Requests
{
    public class ApplicationUserRoomCurrencyUpdateRequest : BaseUpdateRequest
    {
        public int QuantityToChange { get; set; }
    }
}
