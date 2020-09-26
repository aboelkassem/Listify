using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class GenreVM : BaseVM
    {
        public ICollection<PlaylistGenreDTO> PlaylistGenres { get; set; } =
            new List<PlaylistGenreDTO>();
    }
}
