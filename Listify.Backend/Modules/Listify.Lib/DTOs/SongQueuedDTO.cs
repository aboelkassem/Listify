using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Listify.Lib.DTOs
{
    public class SongQueuedDTO : SongRequestDTO
    {
        // this is Value Currently assigned to the song based on incoming
        // and outgoing transactions
        public int WeightedCurrentValue { get; set; }
    }
}
