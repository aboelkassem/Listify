using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ApplicationUserRoomUpdateRequest : BaseUpdateRequest
    {
        public bool IsOnline { get; set; }
    }
}
