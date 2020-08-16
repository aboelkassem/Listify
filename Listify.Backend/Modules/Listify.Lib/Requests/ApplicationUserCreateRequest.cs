using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ApplicationUserCreateRequest : BaseRequest
    {
        public string AspNetUserId { get; set; }
        public string Username { get; set; }
        public string RoomCode { get; set; }
    }
}
