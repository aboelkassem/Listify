using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class ApplicationUserRoomCurrencyVM : BaseVM
    {
        public int Quantity { get; set; }

        public ApplicationUserRoomDTO ApplicationUserRoom { get; set; }
        public CurrencyDTO Currency { get; set; }

        public ICollection<TransactionDTO> Transactions { get; set; } =
            new List<TransactionDTO>();
    }
}
