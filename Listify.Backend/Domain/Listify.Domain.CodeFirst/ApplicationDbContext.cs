using Microsoft.EntityFrameworkCore;
using Listify.Domain.Lib.Entities;
using Listify.Paths;

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
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<LogError> LogsErrors { get; set; }
        public virtual DbSet<LogAPI> LogsAPI { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<PurchasableItem> PurchasableItems { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<PurchasePurchasableItem> PurchasesPurchasableItems { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<SongPlaylist> SongsPlaylists { get; set; }
        public virtual DbSet<SongQueued> SongsQueued { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionSongQueued> TransactionsSongsQueued { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasIndex(s => s.AspNetUserId)
                .IsUnique(true);

            builder.Entity<Room>()
                .HasIndex(s => s.RoomCode)
                .IsUnique(true);

            builder.Entity<ApplicationUser>()
                .HasIndex(s => s.Username)
                .IsUnique(true);

            builder.Entity<ApplicationUserRoomConnection>()
                .HasIndex(s => s.ConnectionId)
                .IsUnique(true);

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
