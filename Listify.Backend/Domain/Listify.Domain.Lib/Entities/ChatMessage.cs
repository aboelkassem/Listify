using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("ChatMessages", Schema ="Listify")]
    public class ChatMessage : BaseEntity
    {
        public string Message { get; set; }

        public Guid ApplicationUserRoomId { get; set; }
        public ApplicationUserRoom ApplicationUserRoom { get; set; }
    }
}
