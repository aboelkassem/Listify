using Listify.Domain.Lib.DTOs;
using Listify.Domain.Lib.Enums;

namespace Listify.Lib.DTOs
{
    public class LogAPIDTO : BaseDTO
    {
        public EndpointType EndPointType { get; set; }
        public int ResponseCode { get; set; }
        public string IPAddress { get; set; }
    }
}
