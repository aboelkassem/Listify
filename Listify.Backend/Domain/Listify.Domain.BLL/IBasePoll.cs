using Listify.Domain.BLL.Events;
using Listify.Domain.BLL.Events.Args;

namespace Listify.Domain.BLL
{
    public interface IBasePoll<T> where T: BasePollingEventArgs
    {
        event ListifyEventHandler<T> PollingEvent;

        void Dispose();
        void Start(int pollingIntervalMS);
        void Stop();
    }
}
