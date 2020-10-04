using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public class RoomGenreVM : BaseVM
    {
        public RoomDTO Room { get; set; }
        public GenreDTO Genre { get; set; }
    }
}
