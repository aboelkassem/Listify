using Listify.Domain.Lib.Requests;

namespace Listify.Lib.Requests
{
    public class PlaylistUpdateRequest : BaseUpdateRequest
    {
        public string PlaylistName { get; set; }

        // if the playlist is currently selected - there can only be 1 selected playlist at a time
        public bool IsSelected { get; set; }
    }
}
