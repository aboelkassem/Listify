using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class ApplicationUserRoomCurrencyRoomVM : BaseVM
    {
        public int Quantity { get; set; }

        public ApplicationUserRoomDTO ApplicationUserRoom { get; set; }
        public CurrencyRoomVM CurrencyRoom { get; set; }

        //public ICollection<TransactionDTO> Transactions { get; set; } =
        //    new List<TransactionDTO>();
    }
}
