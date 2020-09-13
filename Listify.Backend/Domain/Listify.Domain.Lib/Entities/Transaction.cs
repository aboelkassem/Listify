using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    // This could be gaining currency for being in a room
    // or this could be using currency to request/ up-vote / down-vote songs.
    [Table("Transactions", Schema = "Listify")]
    public class Transaction : BaseEntity
    {
        public TransactionType TransactionType { get; set; }
        public int QuantityChange { get; set; }

        public Guid ApplicationUserRoomCurrencyId { get; set; }
        public ApplicationUserRoomCurrencyRoom ApplicationUserRoomCurrency { get; set; }
    }
}
