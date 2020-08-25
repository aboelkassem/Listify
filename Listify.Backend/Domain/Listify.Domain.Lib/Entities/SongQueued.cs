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
        public int WeightedValue { get; set; }  // 3 references
        public bool HasBeenPlayed { get; set; }
        public DateTime TimestampPlayed { get; set; }

        public Guid ApplicationUserId { get; set; }
        public Guid RoomId { get; set; }     // 3 references

        public ApplicationUser ApplicationUser { get; set; }    // 4 references
        public Room Room { get; set; }  // 4 references

        public ICollection<TransactionSongQueued> TransactionsSongQueued { get; set; } =
            new List<TransactionSongQueued>();
    }
}
