using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("Follows", Schema = "Listify")]
    public class Follow: BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid ApplicationUserId { get; set; }

        public Room Room { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
