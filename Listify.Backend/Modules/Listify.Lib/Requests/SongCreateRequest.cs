﻿using Listify.Domain.Lib.Requests;
using System;

namespace Listify.Lib.Requests
{
    public class SongCreateRequest : BaseRequest
    {
        public string SongName { get; set; }
        public string YoutubeId { get; set; }
        public int SongLengthSeconds { get; set; }
    }
}
