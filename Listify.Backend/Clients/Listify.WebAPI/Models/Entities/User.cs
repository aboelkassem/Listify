using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.WebAPI.Models.Entities
{
    public class User
    {
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
