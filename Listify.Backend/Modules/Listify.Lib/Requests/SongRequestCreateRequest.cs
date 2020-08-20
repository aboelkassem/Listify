using Listify.Domain.Lib.Requests;
using System;

namespace Listify.Lib.Requests
{
    public class SongRequestCreateRequest : BaseUpdateRequest
    {
        public Guid SongId { get; set; }
    }
}
