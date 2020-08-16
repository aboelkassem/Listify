using Listify.Domain.Lib.Requests;
using System;

namespace Listify.Lib.Requests
{
    public class SongPlaylistCreateRequest : SongRequestCreateRequest
    {
        public Guid PlaylistId { get; set; }
    }
}
