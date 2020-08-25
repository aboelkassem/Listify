using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("Songs", Schema = "Listify")]
    public class Song : BaseEntity
    {
        public string SongName { get; set; }
        public string YoutubeId { get; set; }
        public int SongLengthSeconds { get; set; }

        public ICollection<SongRequest> SongRequests { get; set; } =
            new List<SongRequest>();
    }
}
