using System;

namespace Listify.Lib.Requests
{
    public class TransactionSongQueuedCreateRequest : TransactionCreateRequest
    {
        public Guid SongQueuedId { get; set; }
    }
}
