using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.WebAPI.Models.Entities
{
    public class Message
    {
        public string MessageRaw { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
