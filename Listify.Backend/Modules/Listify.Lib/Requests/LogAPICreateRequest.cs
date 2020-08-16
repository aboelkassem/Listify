using Listify.Domain.Lib.Enums;
using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class LogAPICreateRequest : BaseRequest
    {
        public EndpointType EndPointType { get; set; }
        public int ResponseCode { get; set; }
        public string IPAddress { get; set; }
    }
}
