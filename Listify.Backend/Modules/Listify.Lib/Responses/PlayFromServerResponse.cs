using Listify.Domain.Lib.Responses;
using Listify.Lib.DTOs;
using Listify.Lib.VMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class PlayFromServerResponse
    {
        public float CurrentTime { get; set; }
        public int PlayerState { get; set; }
        public int Weight { get; set; }

        public SongQueuedVM SongQueued { get; set; }
    }
}
