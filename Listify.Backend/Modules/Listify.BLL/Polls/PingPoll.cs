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
                var clientsPoll = await _service.PingApplicationUsersRoomsConnections();
                if (clientsPoll != null && clientsPoll.Count > 0)
                {
                    FirePollingEvent(this, new PingPollEventArgs
                    {
                        ConnectionsPinged = clientsPoll,
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
