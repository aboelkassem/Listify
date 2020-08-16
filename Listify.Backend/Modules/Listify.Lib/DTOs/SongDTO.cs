using Listify.Domain.Lib.DTOs;

namespace Listify.Lib.DTOs
{
    public class SongDTO : BaseDTO
    {
        public string SongName { get; set; }
        public string YoutubeId { get; set; }
        public int SongLengthSeconds { get; set; }
    }
}
