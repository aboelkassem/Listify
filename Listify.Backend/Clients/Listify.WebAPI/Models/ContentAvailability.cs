using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.WebAPI.Models
{
    public class ContentAvailability
    {
        public bool IsAvailable { get; set; }
        public ValidatedTextType ValidatedTextType { get; set; }
    }
}
