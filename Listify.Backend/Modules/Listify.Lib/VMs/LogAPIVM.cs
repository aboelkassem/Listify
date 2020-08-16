using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public class LogAPIVM : BaseVM
    {
        public EndpointType EndPointType { get; set; }
        public int ResponseCode { get; set; }
        public string IPAddress { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }
    }
}
