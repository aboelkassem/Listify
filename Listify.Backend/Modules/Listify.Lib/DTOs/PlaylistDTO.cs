using Listify.Domain.Lib.DTOs;

namespace Listify.Lib.DTOs
{
    public class PlaylistDTO : BaseDTO
    {
        public string PlaylistName { get; set; }

        // if the playlist is currently selected - there can only be 1 selected playlist at a time
        public bool IsSelected { get; set; }
        public bool IsPublic { get; set; }
        public int NumberOfSongs { get; set; }
        public string PlaylistImageUrl { get; set; }
    }
}
