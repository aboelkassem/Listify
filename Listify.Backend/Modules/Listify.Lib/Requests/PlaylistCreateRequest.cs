using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;

namespace Listify.Lib.Requests
{
    public class PlaylistCreateRequest : BaseRequest
    {
        public Guid Id { get; set; }
        public string PlaylistName { get; set; }
        // if the playlist is currently selected - there can only be 1 selected playlist at a time
        public bool IsSelected { get; set; }
        public bool IsPublic { get; set; }
        public string PlaylistImageUrl { get; set; }

        public ICollection<PlaylistGenreCreateRequest> PlaylistGenres { get; set; } =
            new List<PlaylistGenreCreateRequest>();
    }
}
