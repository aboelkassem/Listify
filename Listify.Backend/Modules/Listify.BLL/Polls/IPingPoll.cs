using Listify.BLL.Events.Args;
using Listify.Domain.BLL;

namespace Listify.BLL.Polls
{
    public interface IPingPoll : IBasePoll<PingPollEventArgs>
    {
    }
}
