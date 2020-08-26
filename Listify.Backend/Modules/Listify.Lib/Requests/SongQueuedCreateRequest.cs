using Listify.Domain.Lib.Entities;
using Listify.Domain.Lib.Requests;
using Listify.Lib.Responses;
using System;

namespace Listify.Lib.Requests
{
    public class SongQueuedCreateRequest : SongRequestCreateRequest
    {
        // this is Value Currently assigned to the song based on incoming
        // and outgoing transactions
        public int QuantityWagered { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid ApplicationUserRoomId { get; set; }

        public YoutubeResults.YoutubeResult SongSearchResult { get; set; }
    }
}
