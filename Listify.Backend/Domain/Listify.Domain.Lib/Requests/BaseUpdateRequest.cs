using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.Requests
{
    public abstract class BaseUpdateRequest : BaseRequest
    {
        public Guid Id { get; set; }
    }
}
