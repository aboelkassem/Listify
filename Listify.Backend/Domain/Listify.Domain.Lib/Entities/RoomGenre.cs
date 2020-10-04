using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("RoomsGenres", Schema = "Listify")]
    public class RoomGenre : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid GenreId { get; set; }

        public Room Room { get; set; }
        public Genre Genre { get; set; }
    }
}
