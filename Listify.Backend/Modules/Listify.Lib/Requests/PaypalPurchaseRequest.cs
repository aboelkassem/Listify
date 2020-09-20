using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.Lib.Requests
{
    public class PaypalPurchaseRequest
    {
        public string payer_id { get; set; }
    }
}
