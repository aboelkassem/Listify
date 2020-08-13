using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("SongRequests", Schema = "Listify")]
    public abstract class SongRequest : BaseEntity
    {
        public Guid SongId { get; set; }
        public Song Song { get; set; }
    }
}
