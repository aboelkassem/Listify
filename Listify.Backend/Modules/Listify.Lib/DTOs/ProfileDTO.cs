using Listify.Domain.Lib.DTOs;
using System;

namespace Listify.Lib.DTOs
{
    public class ProfileDTO : BaseDTO
    {
        public string ProfileTitle { get; set; }
        public string ProfileDescription { get; set; }
        public string AvatarUrl { get; set; }

        public string Username { get; set; }
        public string RoomName { get; set; }
        public string RoomUrl { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
