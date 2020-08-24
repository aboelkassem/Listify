using Listify.Domain.Lib.DTOs;
using Listify.Domain.Lib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Lib.DTOs
{
    public abstract class SongRequestDTO : BaseDTO
    {
        public SongRequestType SongRequestType { get; set; }
    }
}
