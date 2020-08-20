using Listify.Domain.Lib.Requests;
using Listify.Lib.Responses;
using System;

namespace Listify.Lib.Requests
{
    public class SongPlaylistCreateRequest : BaseRequest
    {
        public Guid PlaylistId { get; set; }
        public YoutubeResults.YoutubeResult SongSearchResult { get; set; }
    }
}
