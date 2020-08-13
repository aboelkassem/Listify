using Microsoft.EntityFrameworkCore;
using Listify.Domain.Lib.Entities;
using Listify.WebAPI;

namespace Listify.Domain.CodeFirst
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<ApplicationUserRoom> ApplicationUsersRooms { get; set; }
        public virtual DbSet<ApplicationUserRoomConnection> ApplicationUsersRoomsConnections { get; set; }
        public virtual DbSet<ApplicationUserRoomCurrency> ApplicationUsersRoomsCurrencies { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<SongPlaylist> SongsPlaylists { get; set; }
        public virtual DbSet<SongQueued> SongsQueued { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionSongQueued> TransactionsSongsQueued { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Room>()
                .HasMany(s => s.ApplicationUsersRooms)
                .WithOne(s => s.Room)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Room>()
                .HasMany(s => s.SongsQueued)
                .WithOne(s => s.Room)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SongQueued>()
                .HasOne(s => s.ApplicationUser)
                .WithMany(s => s.SongsQueued)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TransactionSongQueued>()
                .HasOne(s => s.SongQueued)
                .WithMany(s => s.TransactionsSongQueued)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Currency>()
                .HasMany(s => s.ApplicationUsersRoomsCurrencies)
                .WithOne(s => s.Currency)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Globals.DEV_CONNECTION_STRING);
        }
    }
}
