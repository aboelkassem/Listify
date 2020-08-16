using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        // Default starting value is 100 songs
        public int SongPoolCountSongsMax { get; set; } = 100;

        public Room Room { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; } =
            new List<ChatMessage>();

        public ICollection<ApplicationUserRoom> ApplicationUsersRooms { get; set; } =
            new List<ApplicationUserRoom>();

        public ICollection<Playlist> Playlists { get; set; } =
            new List<Playlist>();

        public ICollection<SongQueued> SongsQueued { get; set; } =
            new List<SongQueued>();

        public ICollection<LogAPI> LogsAPI { get; set; } =
            new List<LogAPI>();

        public ICollection<LogError> LogsErrors { get; set; } =
            new List<LogError>();
    }
}
