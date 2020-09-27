using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.VMs
{
    public class ProfileVM : BaseVM
    {
        public string ProfileTitle { get; set; }
        public string ProfileDescription { get; set; }
        public string AvatarUrl { get; set; }

        public string Username { get; set; }
        public string RoomName { get; set; }
        public string RoomUrl { get; set; }
        public DateTime DateJoined { get; set; }

        public ICollection<PlaylistCommunityDTO> PlaylistsCommunity { get; set; } =
            new List<PlaylistCommunityDTO>();
    }
}
