using Listify.Domain.Lib.Requests;
using Listify.Lib.DTOs;
using Listify.Lib.VMs;
using System;

namespace Listify.Lib.Requests
{
    public class ServerStateRequest : BaseRequest
    {
        public string ConnectionId { get; set; }
    }
}
