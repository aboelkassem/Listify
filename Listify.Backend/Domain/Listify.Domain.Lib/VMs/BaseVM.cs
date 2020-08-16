using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.VMs
{
    public abstract class BaseVM
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
