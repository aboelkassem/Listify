using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("ApplicationUsersRoomsCurrenciesRoom", Schema = "Listify")]
    public class ApplicationUserRoomCurrencyRoom : BaseEntity
    {
        public float Quantity { get; set; }

        public Guid ApplicationUserRoomId { get; set; }
        public Guid CurrencyRoomId { get; set; }

        public ApplicationUserRoom ApplicationUserRoom { get; set; }
        public CurrencyRoom CurrencyRoom { get; set; }

        public ICollection<Transaction> Transactions { get; set; } =
            new List<Transaction>();
    }
}
