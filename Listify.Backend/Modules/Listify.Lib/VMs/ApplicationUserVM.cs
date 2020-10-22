using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using System;
using System.Collections.Generic;

namespace Listify.Lib.VMs
{
    public class ApplicationUserVM : BaseVM
    {
        public string AspNetUserId { get; set; }
        public string Username { get; set; }

        // This is the number of Song Pools/ Playlists the user has, it should be purchasable
        // Default starting value is 1 song pool
        public int PlaylistCountMax { get; set; }

        // This is the number of Songs That are allowed in each Song Pool, it should be purchasable
        // Default starting value is 100 songs
        public int PlaylistSongCount { get; set; }
        public int QueueSongsCount { get; set; }
        public string ChatColor { get; set; }
        public DateTime DateJoined { get; set; }
        public string ProfileTitle { get; set; }
        public string ProfileDescription { get; set; }
        public string ProfileImageUrl { get; set; }

        public RoomVM Room { get; set; }

        public ICollection<ApplicationUserRoomDTO> ApplicationUsersRooms { get; set; } =
            new List<ApplicationUserRoomDTO>();

        public ICollection<PlaylistDTO> Playlists { get; set; } =
            new List<PlaylistDTO>();

        public ICollection<SongQueuedDTO> SongsQueued { get; set; } =
            new List<SongQueuedDTO>();
    }
}
