using Listify.Domain.BLL.Events;
using Listify.Domain.BLL.Events.Args;
using System.Threading.Tasks;

namespace Listify.Domain.BLL
{
    public interface IBasePoll<T> where T: BasePollingEventArgs
    {
        event ListifyEventHandler<T> PollingEvent;

        void Dispose();
        void Start(int pollingIntervalMS);
        Task StopAync();
    }
}
