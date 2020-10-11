using Listify.Domain.Lib.Requests;
using System;

namespace Listify.Lib.Requests
{
    public class SongCreateRequest : BaseRequest
    {
        public string SongName { get; set; }
        public string YoutubeId { get; set; }
        public int SongLengthSeconds { get; set; }

        // Default Thumbnail
        public string ThumbnailUrl { get; set; }
        public int ThumbnailWidth { get; set; }
        public int ThumbnailHeight { get; set; }
    }
}
