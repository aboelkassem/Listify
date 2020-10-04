using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("Genres", Schema = "Listify")]
    public class Genre : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<PlaylistGenre> PlaylistGenres { get; set; } =
            new List<PlaylistGenre>();

        public ICollection<RoomGenre> RoomGenres { get; set; } =
            new List<RoomGenre>();
    }
}
