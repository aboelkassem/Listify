using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class SongQueuedVM : SongRequestVM
    {
        // this is Value Currently assigned to the song based on incoming
        // and outgoing transactions
        public int WeightedCurrentValue { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }
        public RoomDTO Room { get; set; }

        public ICollection<TransactionSongQueuedDTO> TransactionsSongQueued { get; set; } =
            new List<TransactionSongQueuedDTO>();
    }
}
