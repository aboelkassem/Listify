using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Domain.BLL.Args
{
    public class BasePollingEventArgs
    {
        public PollingEventType PollingEventType { get; set; }
    }
}
