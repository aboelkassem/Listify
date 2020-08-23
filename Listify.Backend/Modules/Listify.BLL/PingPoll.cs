using Listify.BLL.Args;
using Listify.DAL;
using Listify.Domain.BLL;
using Listify.Domain.Lib.Enums;
using System.Threading.Tasks;

namespace Listify.BLL
{
    public class PingPoll : BasePoll<PingPollEventArgs>
    {
        public PingPoll(IListifyServices service): base(service)
        {

        }

        protected override async Task TimerTickEvents()
        {
            try
            {
                var pingPoll = await _service.PingApplicationUsersRoomsConnections();
                if (pingPoll != null && pingPoll.Count > 0)
                {
                    FirePollingEvent(this, new PingPollEventArgs
                    {
                        ConnectionsPinged = pingPoll,
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
