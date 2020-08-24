using Listify.Domain.Lib.Enums;

namespace Listify.Domain.BLL.Events.Args
{
    public abstract class BasePollingEventArgs : BaseEventArgs
    {
        public PollingEventType PollingEventType { get; set; }
    }
}
