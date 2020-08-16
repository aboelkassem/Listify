﻿using Listify.Domain.Lib.DTOs;

namespace Listify.Lib.DTOs
{
    public class ApplicationUserDTO : BaseDTO
    {
        public string AspNetUserId { get; set; }
        public string Username { get; set; }

        // This is the number of Song Pools/ Playlists the user has, it should be purchasable
        // Default starting value is 1 song pool
        public int PlaylistCountMax { get; set; }

        // This is the number of Songs That are allowed in each Song Pool, it should be purchasable
        // Default starting value is 100 songs
        public int SongPoolCountSongsMax { get; set; }
    }
}
