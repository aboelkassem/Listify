using Listify.Domain.Lib.Requests;
using System;

namespace Listify.Lib.Requests
{
    public class SongQueuedCreateRequest : SongRequestCreateRequest
    {
        // this is Value Currently assigned to the song based on incoming
        // and outgoing transactions
        public int WeightedCurrentValue { get; set; }

        public Guid RoomId { get; set; }
    }
}
