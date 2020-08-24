using Listify.Domain.BLL.Events.Args;

namespace Listify.Domain.BLL.Events
{
    public delegate void ListifyEventHandler<T>(object sender, T args) where T : BaseEventArgs;
}
