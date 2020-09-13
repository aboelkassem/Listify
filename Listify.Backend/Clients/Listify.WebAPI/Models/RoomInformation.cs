using Listify.Lib.DTOs;
using Listify.Lib.VMs;

namespace Listify.WebAPI.Models
{
    public class RoomInformation
    {
        public RoomDTO Room { get; set; }
        public ApplicationUserRoomVM ApplicationUserRoom { get; set; }
        public ApplicationUserRoomCurrencyRoomVM[] ApplicationUserRoomCurrenciesRoom { get; set; }
        public ApplicationUserDTO RoomOwner { get; set; }
    }
}
