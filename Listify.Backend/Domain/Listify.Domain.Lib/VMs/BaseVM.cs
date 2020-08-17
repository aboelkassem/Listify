using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.Lib.VMs
{
    public abstract class BaseVM
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
