using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;

namespace Listify.Lib.VMs
{
    public abstract class SongRequestVM : BaseVM
    {
        public SongRequestType SongRequestType { get; set; }
        public SongDTO Song { get; set; }
    }
}
