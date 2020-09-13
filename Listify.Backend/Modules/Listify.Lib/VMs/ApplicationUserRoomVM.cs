using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class ApplicationUserRoomVM : BaseVM
    {
        public bool IsOnline { get; set; }
        public bool IsOwner { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }
        public RoomDTO Room { get; set; }

        public ICollection<ChatMessageDTO> ChatMessages { get; set; } =
            new List<ChatMessageDTO>();

        public ICollection<ApplicationUserRoomCurrencyRoomDTO> ApplicationUsersRoomsCurrenciesRoom { get; set; } =
            new List<ApplicationUserRoomCurrencyRoomDTO>();

        public ICollection<ApplicationUserRoomConnectionDTO> ApplicationUsersRoomsConnections { get; set; } =
            new List<ApplicationUserRoomConnectionDTO>();
    }
}
