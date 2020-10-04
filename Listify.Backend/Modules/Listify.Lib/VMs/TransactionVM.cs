using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Lib.VMs
{
    // This could be gaining currency for being in a room
    // or this could be using currency to request/ up-vote / down-vote songs.
    public class TransactionVM : BaseVM
    {
        public TransactionType TransactionType { get; set; }
        public int QuantityChanged { get; set; }

        public ApplicationUserRoomCurrencyRoomDTO ApplicationUserRoomCurrency { get; set; }
    }
}
