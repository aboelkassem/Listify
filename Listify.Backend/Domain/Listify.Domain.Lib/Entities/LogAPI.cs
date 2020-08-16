using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Domain.Lib.Entities
{
    [Table("LogsAPI", Schema = "Listify")]
    public class LogAPI : BaseEntity
    {
        public EndpointType EndPointType { get; set; }
        public int ResponseCode { get; set; }
        public string IPAddress { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
