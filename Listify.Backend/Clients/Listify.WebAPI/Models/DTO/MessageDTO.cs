using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.WebAPI.Models.DTO
{
    public class MessageDTO
    {
        public string Message { get; set; }
        public Guid MyProperty { get; set; }
    }
}
