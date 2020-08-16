using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("LogErrors", Schema = "Listify")]
    public class LogError : BaseEntity
    {
        public string Exception { get; set; }
        public string IPAddress { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
