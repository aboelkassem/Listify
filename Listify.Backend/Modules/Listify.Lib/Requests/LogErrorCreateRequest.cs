using Listify.Domain.Lib.Requests;

namespace Listify.Lib.Requests
{
    public class LogErrorCreateRequest : BaseRequest
    {
        public string Exception { get; set; }
        public string IPAddress { get; set; }
    }
}
