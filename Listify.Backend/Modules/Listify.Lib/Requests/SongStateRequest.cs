using Listify.Domain.Lib.Requests;
using Listify.Lib.VMs;
using System;

namespace Listify.Lib.Requests
{
    public class SongStateRequest: BaseRequest
    {
        public Guid SongQueuedId { get; set; }
        public Guid SongId { get; set; }
        public float CurrentTime { get; set; }
        public int PlayerState { get; set; }
        public string ConnectionId { get; set; }

        public SongVM Song { get; set; }
    }
}
