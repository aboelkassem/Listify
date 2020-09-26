using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Listify.Domain.Lib.Entities
{
    [Table("ApplicationUsers", Schema = "Listify")]
    public class ApplicationUser : BaseEntity
    {
        public string AspNetUserId { get; set; }
        public string Username { get; set; }

        // This is the number of Song Pools/ Playlists the user has, it should be purchasable
        // Default starting value is 1 song pool
        public int PlaylistCountMax { get; set; } = 1;

        // This is the number of Songs That are allowed in each Song Pool, it should be purchasable
        // Default starting value is 30 songs
        public int PlaylistSongCount { get; set; } = 30;
        //public int QueueSongsCount { get; set; } = 50;
        public int QueueCount { get; set; } = 30;
        public string ChatColor { get; set; } = "#000000";
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;

        public Room Room { get; set; }

        public ICollection<ApplicationUserRoom> ApplicationUsersRooms { get; set; } =
            new List<ApplicationUserRoom>();

        public ICollection<Playlist> Playlists { get; set; } =
            new List<Playlist>();

        public ICollection<SongQueued> SongsQueued { get; set; } =
            new List<SongQueued>();

        public ICollection<Purchase> Purchases { get; set; } =
            new List<Purchase>();

        public ICollection<LogAPI> LogsAPI { get; set; } =
            new List<LogAPI>();

        public ICollection<LogError> LogsErrors { get; set; } =
            new List<LogError>();
    }
}
