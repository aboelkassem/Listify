using Listify.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class WagerQuantitySongQueuedRquest
    {
        public SongQueuedDTO SongQueued { get; set; }
        public ApplicationUserRoomDTO ApplicationUserRoom { get; set; }
        public ApplicationUserRoomCurrencyRoomDTO ApplicationUserRoomCurrencyRoom { get; set; }
        public int Quantity { get; set; }
    }
}
