using Listify.BLL.Events.Args;
using Listify.BLL.Polls;
using Listify.DAL;
using Listify.Domain.BLL;
using Listify.Domain.Lib.Enums;
using System.Threading.Tasks;

namespace Listify.BLL
{
    public class PingPoll : BasePoll<PingPollEventArgs>, IPingPoll
    {
        public PingPoll(IListifyServices service): base(service)
        {

        }

        protected override async Task TimerTickEvents()
        {
            try
            {
                var clientsPinged = await _service.PingApplicationUsersRoomsConnections();
                if (clientsPinged != null && clientsPinged.Count > 0)
                {
                    FirePollingEvent(this, new PingPollEventArgs
                    {
                        ConnectionsPinged = clientsPinged,
                        PollingEventType = PollingEventType.PingPoll
                    });
                }
            }
            catch
            {
            }
        }
    }
}
