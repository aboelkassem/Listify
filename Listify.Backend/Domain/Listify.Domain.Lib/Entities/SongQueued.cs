using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    public class SongQueued : SongRequest
    {
        // this is Value Currently assigned to the song based on incoming
        // and outgoing transactions
        public int WeightedValue { get; set; }

        public Guid ApplicationUserId { get; set; }
        public Guid RoomId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Room Room { get; set; }

        public ICollection<TransactionSongQueued> TransactionsSongQueued { get; set; } =
            new List<TransactionSongQueued>();
    }
}
