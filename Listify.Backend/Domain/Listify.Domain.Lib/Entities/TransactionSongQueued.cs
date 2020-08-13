using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    public class TransactionSongQueued : Transaction
    {
        public Guid SongQueuedId { get; set; }
        public SongQueued SongQueued { get; set; }
    }
}
