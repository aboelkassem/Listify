using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public class FollowVM : BaseVM
    {
        public RoomDTO Room { get; set; }
        public ApplicationUserDTO ApplicationUser { get; set; }
    }
}
