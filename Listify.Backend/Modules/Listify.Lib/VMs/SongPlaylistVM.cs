using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public class SongPlaylistVM : SongRequestVM
    {
        // This is the counter to track the number of times an individual song
        // has been played in the playlist - this is to make sure that all songs 
        // are played before repeating.
        public int PlayCount { get; set; }

        public PlaylistDTO Playlist { get; set; }
    }
}
