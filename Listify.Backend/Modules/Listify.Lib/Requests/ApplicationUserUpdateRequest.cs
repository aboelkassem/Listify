using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ApplicationUserUpdateRequest : BaseUpdateRequest
    {
        public string Username { get; set; }
    }
}
