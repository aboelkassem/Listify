using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ChatMessageCreateRequest : BaseRequest
    {
        public string Message { get; set; }

        public Guid ApplicationUserRoomId { get; set; }
    }
}
