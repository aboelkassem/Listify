using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    public class SongPlaylist : SongRequest
    {
        // This is the counter to track the number of times an individual song
        // has been played in the playlist - this is to make sure that all songs 
        // are played before repeating.
        public int PlayCount { get; set; }

        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
