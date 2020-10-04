using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("Playlists", Schema = "Listify")]
    public class Playlist : BaseEntity
    {
        public string PlaylistName { get; set; }

        // if the playlist is currently selected - there can only be 1 selected playlist at a time
        public bool IsSelected { get; set; }
        public bool IsPublic { get; set; }
        public string PlaylistImageUrl { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<SongPlaylist> SongsPlaylist { get; set; } =
            new List<SongPlaylist>();

        public ICollection<PlaylistGenre> PlaylistGenres { get; set; } =
            new List<PlaylistGenre>();
    }
}
