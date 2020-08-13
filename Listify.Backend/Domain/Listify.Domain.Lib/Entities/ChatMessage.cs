using Listify.Domain.Lib.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.Doimain.Lib.Entities
{
    [Table("ChatMessages", Schema ="Listify")]
    public class ChatMessage : BaseEntity
    {
        public string Message { get; set; }

        public Guid ApplicationUserRoomId { get; set; }
        public ApplicationUserRoom ApplicationUserRoom { get; set; }
    }
}
