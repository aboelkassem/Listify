using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("ApplicationUsersRoomsCurrencies", Schema = "Listify")]
    public class ApplicationUserRoomCurrency : BaseEntity
    {
        public int Quantity { get; set; }

        public Guid ApplicationUserRoomId { get; set; }
        public Guid CurrencyId { get; set; }

        public ApplicationUserRoom ApplicationUserRoom { get; set; }
        public Currency Currency { get; set; }

        public ICollection<Transaction> Transactions { get; set; } =
            new List<Transaction>();
    }
}
