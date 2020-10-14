using Listify.Domain.Lib.Requests;
using Listify.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ProfileUpdateRequest: BaseUpdateRequest
    {
        public string Username { get; set; }
        public string ProfileTitle { get; set; }
        public string ProfileDescription { get; set; }
        public string ProfileImageUrl { get; set; }
        public int NumberFollows { get; set; }
        public RoomDTO Room { get; set; }
        public DateTime DateJoined { get; set; }
        public PlaylistDTO[] Playlists { get; set; }
    }
}
