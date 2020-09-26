using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public class PlaylistGenreVM : BaseVM
    {
        public PlaylistDTO Playlist { get; set; }
        public GenreDTO Genre { get; set; }
    }
}
