using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("PlaylistGenres", Schema = "Listify")]
    public class PlaylistGenre: BaseEntity
    {
        public Guid PlaylistId { get; set; }
        public Guid GenreId { get; set; }

        public Playlist Playlist { get; set; }
        public Genre Genre { get; set; }
    }
}
