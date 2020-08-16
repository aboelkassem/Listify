using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public abstract class SongRequestVM : BaseVM
    {
        public SongDTO Song { get; set; }
    }
}
