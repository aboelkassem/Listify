using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class ProfileVM : BaseVM
    {
        public string Username { get; set; }
        public string ProfileTitle { get; set; }
        public string ProfileDescription { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime DateJoined { get; set; }
        public int NumberFollows { get; set; }

        public RoomVM Room{ get; set; }
        public ICollection<PlaylistVM> Playlists { get; set; } =
            new List<PlaylistVM>();
    }
}
