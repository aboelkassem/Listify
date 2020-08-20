using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class YoutubeSearchResponse
    {
        public Guid Id { get; set; }
        public string SongName { get; set; }
        public int LengthSec { get; set; }
        public string VideoId { get; set; }
    }
}
