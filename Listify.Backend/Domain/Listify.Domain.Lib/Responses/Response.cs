using Listify.Domain.Lib.VMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.Responses
{
    public class Response<T> where T: BaseVM
    {
        public ICollection<T> Data { get; set; } =
            new List<T>();
    }
}
