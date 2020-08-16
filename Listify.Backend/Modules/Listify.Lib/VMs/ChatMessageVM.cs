using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System;

namespace Listify.Lib.VMs
{
    public class ChatMessageVM : BaseVM
    {
        public string Message { get; set; }

        public ApplicationUserRoomDTO ApplicationUserRoom { get; set; }
    }
}
