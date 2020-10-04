using Listify.Lib.DTOs;
using Listify.Lib.VMs;

namespace Listify.WebAPI.Models
{
    public class RoomInformation
    {
        public RoomVM Room { get; set; }
        public ApplicationUserDTO RoomOwner { get; set; }
        public ApplicationUserRoomVM ApplicationUserRoom { get; set; }
        public ApplicationUserRoomCurrencyRoomVM[] ApplicationUserRoomCurrenciesRoom { get; set; }
    }
}
