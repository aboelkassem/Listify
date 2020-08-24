using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("SongRequests", Schema = "Listify")]
    public abstract class SongRequest : BaseEntity
    {
        public SongRequestType SongRequestType { get; set; }
        public Guid SongId { get; set; }    // 2 references
        public Song Song { get; set; } // 7 references
    }
}
