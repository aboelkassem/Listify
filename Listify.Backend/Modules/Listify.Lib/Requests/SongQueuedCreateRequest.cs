using Listify.Lib.DTOs;
using Listify.Lib.Responses;
using System;

namespace Listify.Lib.Requests
{
    public class SongQueuedCreateRequest : SongRequestCreateRequest
    {
        public YoutubeResults.YoutubeResult SongSearchResult { get; set; }
    }
}
