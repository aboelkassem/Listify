using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class SongStateRequest: BaseRequest
    {
        public Guid SongQueuedId { get; set; }
        public float CurrentTime { get; set; }
        public int PlayerState { get; set; }
        public string ConnectionId { get; set; }
    }
}
