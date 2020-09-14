using Microsoft.EntityFrameworkCore;
using Listify.Domain.Lib.Entities;
using Listify.Paths;

namespace Listify.Domain.CodeFirst
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<ApplicationUserRoom> ApplicationUsersRooms { get; set; }
        public virtual DbSet<ApplicationUserRoomConnection> ApplicationUsersRoomsConnections { get; set; }
        public virtual DbSet<ApplicationUserRoomCurrencyRoom> ApplicationUsersRoomsCurrenciesRooms { get; set; }
        public virtual DbSet<CurrencyRoom> CurrenciesRooms { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<LogError> LogsErrors { get; set; }
        public virtual DbSet<LogAPI> LogsAPI { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<PurchasableItem> PurchasableItems { get; set; }
        public virtual DbSet<PurchaseLineItem> PurchaseLineItems { get; set; }
        public virtual DbSet<PurchaseLineItemCurrency> PurchaseLineItemsCurrencies { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
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

            builder.Entity<Room>()
                .HasMany(s => s.CurrenciesRoom)
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
                .HasMany(s => s.CurrenciesRoom)
                .WithOne(s => s.Currency)
                .OnDelete(DeleteBehavior.Restrict);


            // Seed Some Data 
            builder.Entity<Currency>()
               .HasData(new Currency
               {
                   Active = true,
                   CurrencyName = "Tokens",
                   Id = new System.Guid("7385DB66-C5D6-4F99-84DC-74CF9695A459"),
                   QuantityIncreasePerTick = 2,
                   TimeSecBetweenTick = 30,
                   Weight = 2,
                   TimeStamp = System.DateTime.UtcNow
               });

            builder.Entity<PurchasableItem>()
               .HasData(new PurchasableItem
               {
                   Active = true,
                   Id = new System.Guid("2BAF4F54-875D-4668-8843-8E765E66EB00"),
                   DiscountApplied = 0,
                   PurchasableItemName = "1 Playlist",
                   PurchasableItemType = Lib.Enums.PurchasableItemType.Playlist,
                   Quantity = 1,
                   TimeStamp = System.DateTime.UtcNow,
                   UnitCost = 1,
                   ImageUri = "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001888/Listify%20Photos/1-playlist_gzqfcr.jpg"
               },
               new PurchasableItem
               {
                   Active = true,
                   Id = new System.Guid("58542258-D6B4-490E-835A-3E78EC8C9D2D"),
                   DiscountApplied = 0,
                   PurchasableItemName = "Pack of 3 Playlist",
                   PurchasableItemType = Lib.Enums.PurchasableItemType.Playlist,
                   Quantity = 3,
                   TimeStamp = System.DateTime.UtcNow,
                   UnitCost = 2,
                   ImageUri = "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001883/Listify%20Photos/3-playlist_p0sg3o.jpg"
               },
               new PurchasableItem
               {
                   Active = true,
                   Id = new System.Guid("610DFA85-A0D3-4C36-AB8E-0153FF3742CE"),
                   DiscountApplied = 0,
                   PurchasableItemName = "Pack of 5 Playlist",
                   PurchasableItemType = Lib.Enums.PurchasableItemType.Playlist,
                   Quantity = 5,
                   TimeStamp = System.DateTime.UtcNow,
                   UnitCost = 3,
                   ImageUri = "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001889/Listify%20Photos/5-playlist_bqmufv.jpg"
               },
               new PurchasableItem
               {
                   Active = true,
                   Id = new System.Guid("F217AA5A-A01F-4B7E-891B-B7D9210E8A11"),
                   DiscountApplied = 0,
                   PurchasableItemName = "Pack of 10 Playlist",
                   PurchasableItemType = Lib.Enums.PurchasableItemType.Playlist,
                   Quantity = 10,
                   TimeStamp = System.DateTime.UtcNow,
                   UnitCost = 5,
                   ImageUri = "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001889/Listify%20Photos/10-playlist_myaf2g.jpg"
               },
               new PurchasableItem
               {
                   Active = true,
                   Id = new System.Guid("5552E1CC-2A96-4590-B2CA-D3305C229353"),
                   DiscountApplied = 0,
                   PurchasableItemName = "15 Additional Songs Per Playlist",
                   PurchasableItemType = Lib.Enums.PurchasableItemType.PlyalistSongs,
                   Quantity = 15,
                   TimeStamp = System.DateTime.UtcNow,
                   UnitCost = 1,
                   ImageUri = "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001886/Listify%20Photos/15-songs_ut0thz.jpg"
               },
               new PurchasableItem
               {
                   Active = true,
                   Id = new System.Guid("018B97A0-C7EA-43E4-9531-3E48DFB3FA6E"),
                   DiscountApplied = 0,
                   PurchasableItemName = "40 Additional Songs Per Playlist",
                   PurchasableItemType = Lib.Enums.PurchasableItemType.PlyalistSongs,
                   Quantity = 40,
                   TimeStamp = System.DateTime.UtcNow,
                   UnitCost = 2,
                   ImageUri = "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001886/Listify%20Photos/40-songs_xx4al5.jpg"
               },
               new PurchasableItem
               {
                   Active = true,
                   Id = new System.Guid("5A72BC3C-7494-484D-A30A-5E6A6C698B0D"),
                   DiscountApplied = 0,
                   PurchasableItemName = "80 Additional Songs Per Playlist",
                   PurchasableItemType = Lib.Enums.PurchasableItemType.PlyalistSongs,
                   Quantity = 80,
                   TimeStamp = System.DateTime.UtcNow,
                   UnitCost = 3,
                   ImageUri = "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001884/Listify%20Photos/80-songs_gdpufy.jpg"
               },
               new PurchasableItem
               {
                   Active = true,
                   Id = new System.Guid("01237025-4AE3-4C73-8AD8-A94C67DE8116"),
                   DiscountApplied = 0,
                   PurchasableItemName = "160 Additional Songs Per Playlist",
                   PurchasableItemType = Lib.Enums.PurchasableItemType.PlyalistSongs,
                   Quantity = 160,
                   TimeStamp = System.DateTime.UtcNow,
                   UnitCost = 5,
                   ImageUri = "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001888/Listify%20Photos/160-songs_ztgjsd.jpg"
               },
               new PurchasableItem
               {
                   Active = true,
                   Id = new System.Guid("AA147747-3010-4047-8103-B1B50A93BF7F"),
                   DiscountApplied = 0,
                   PurchasableItemName = "40 Currencies Per Room",
                   PurchasableItemType = Lib.Enums.PurchasableItemType.PurchaseCurrency,
                   Quantity = 40,
                   TimeStamp = System.DateTime.UtcNow,
                   UnitCost = 1,
                   ImageUri = "https://res.cloudinary.com/dvdcninhs/image/upload/v1600001885/Listify%20Photos/40-tokens_ppx2qi.jpg"
               });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Globals.DEV_CONNECTION_STRING);
        }
    }
}
