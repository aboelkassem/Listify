using Listify.Domain.Lib.VMs;

namespace Listify.Lib.VMs
{
    public class ChatMessageVM : BaseVM
    {
        public string Message { get; set; }

        public ApplicationUserRoomVM ApplicationUserRoom { get; set; }
    }
}
