using Listify.Domain.Lib.Requests;
using Listify.Lib.Responses;

namespace Listify.Lib.Requests
{
    public class SongQueuedCreateRequest : BaseRequest
    {
        public YoutubeResults.YoutubeResult SongSearchResult { get; set; }
    }
}
