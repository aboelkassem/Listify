using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("SongsRequestsHistory", Schema = "Listify")]
    public class SongRequestHistory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new Guid Id { get; set; }
        public SongRequestType SongRequestType { get; set; }
        // this is Value Currently assigned to the song based on incoming
        // and outgoing transactions
        public int WeightedValue { get; set; }

        public Guid SongId { get; set; }
        public Song Song { get; set; }
        public DateTime PlayedTimestamp { get; set; }

        public Guid ApplicationUserId { get; set; }
        public Guid RoomId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Room Room { get; set; }
    }
}
