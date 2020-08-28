using Listify.Domain.Lib.Entities;
using Listify.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class YoutubeResults
    {
        public ICollection<YoutubeResult> Results { get; set; } =
            new List<YoutubeResult>();

        public class YoutubeResult
        {
            public Guid Id { get; set; }
            public string SongName { get; set; }
            public int LengthSec { get; set; }
            public string VideoId { get; set; }
            public ApplicationUserRoomCurrencyDTO ApplicationUserRoomCurrency { get; set; }
        }
    }
}
