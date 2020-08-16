using Listify.Domain.Lib.Requests;
using System;

namespace Listify.Lib.Requests
{
    public class SongRequestCreateRequest : BaseRequest
    {
        public Guid SongId { get; set; }
    }
}
