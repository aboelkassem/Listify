using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class SongVM : BaseVM
    {
        public string SongName { get; set; }
        public string YoutubeId { get; set; }
        public int SongLengthSeconds { get; set; }

        public ICollection<SongRequestDTO> SongRequests { get; set; } =
            new List<SongRequestDTO>();
    }
}
