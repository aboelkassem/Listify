using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public class LogErrorVM : BaseVM
    {
        public string Exception { get; set; }
        public string IPAddress { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }
    }
}
