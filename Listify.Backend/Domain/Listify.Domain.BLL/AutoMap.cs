using AutoMapper;
using Listify.Domain.Lib.Entities;
using Listify.Lib.DTOs;
using Listify.Lib.Requests;
using Listify.Lib.VMs;

namespace Listify.Domain.BLL
{
    public static class AutoMap
    {
        public static IMapper CreateAutoMapper()
        {
            return new MapperConfiguration(c =>
            {
                c.CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
                c.CreateMap<ApplicationUser, ApplicationUserVM>().ReverseMap();
                c.CreateMap<ApplicationUserVM, ApplicationUserDTO>().ReverseMap();
                c.CreateMap<ApplicationUserCreateRequest, ApplicationUser>();
                c.CreateMap<ApplicationUserUpdateRequest, ApplicationUser>();

                c.CreateMap<ApplicationUser, ProfileVM>().ReverseMap();

                c.CreateMap<ApplicationUserRoom, ApplicationUserRoomDTO>().ReverseMap();
                c.CreateMap<ApplicationUserRoom, ApplicationUserRoomVM>().ReverseMap();
                c.CreateMap<ApplicationUserRoomVM, ApplicationUserRoomDTO>().ReverseMap();
                c.CreateMap<ApplicationUserRoomCreateRequest, ApplicationUserRoom>();
                c.CreateMap<ApplicationUserRoomUpdateRequest, ApplicationUserRoom>();

                c.CreateMap<ApplicationUserRoomConnection, ApplicationUserRoomConnectionDTO>().ReverseMap();
                c.CreateMap<ApplicationUserRoomConnection, ApplicationUserRoomConnectionVM>().ReverseMap();
                c.CreateMap<ApplicationUserRoomConnectionVM, ApplicationUserRoomConnectionDTO>().ReverseMap();
                c.CreateMap<ApplicationUserRoomConnectionCreateRequest, ApplicationUserRoomConnection>();
                c.CreateMap<ApplicationUserRoomConnectionUpdateRequest, ApplicationUserRoomConnection>();

                c.CreateMap<ApplicationUserRoomCurrencyRoom, ApplicationUserRoomCurrencyRoomDTO>().ReverseMap();
                c.CreateMap<ApplicationUserRoomCurrencyRoom, ApplicationUserRoomCurrencyRoomVM>().ReverseMap();
                c.CreateMap<ApplicationUserRoomCurrencyRoomVM, ApplicationUserRoomCurrencyRoomDTO>().ReverseMap();
                c.CreateMap<ApplicationUserRoomCurrencyRoomCreateRequest, ApplicationUserRoomCurrencyRoom>();

                c.CreateMap<ChatMessage, ChatMessageDTO>().ReverseMap();
                c.CreateMap<ChatMessage, ChatMessageVM>().ReverseMap();
                c.CreateMap<ChatMessageVM, ChatMessageDTO>().ReverseMap();
                c.CreateMap<ChatMessageCreateRequest, ChatMessage>();

                c.CreateMap<Currency, CurrencyDTO>().ReverseMap();
                c.CreateMap<Currency, CurrencyVM>().ReverseMap();
                c.CreateMap<CurrencyVM, CurrencyDTO>().ReverseMap();

                c.CreateMap<CurrencyRoom, CurrencyRoomDTO>().ReverseMap();
                c.CreateMap<CurrencyRoom, CurrencyRoomVM>().ReverseMap();
                c.CreateMap<CurrencyRoomVM, CurrencyRoomDTO>().ReverseMap();
                c.CreateMap<CurrencyRoomCreateRequest, CurrencyRoom>();

                c.CreateMap<Playlist, PlaylistDTO>().ReverseMap();
                c.CreateMap<Playlist, PlaylistVM>().ReverseMap();
                c.CreateMap<PlaylistVM, PlaylistDTO>().ReverseMap();
                c.CreateMap<PlaylistCreateRequest, Playlist>();

                c.CreateMap<PurchasableItem, PurchasableItemDTO>().ReverseMap();
                c.CreateMap<PurchasableItem, PurchasableItemVM>().ReverseMap();
                c.CreateMap<PurchasableItemVM, PurchasableItemDTO>().ReverseMap();
                c.CreateMap<PurchasableItemCreateRequest, PurchasableItem>();

                c.CreateMap<Purchase, PurchaseDTO>().ReverseMap();
                c.CreateMap<Purchase, PurchaseVM>().ReverseMap();
                c.CreateMap<PurchaseVM, PurchaseDTO>().ReverseMap();
                c.CreateMap<PurchaseCreateRequest, Purchase>();

                c.CreateMap<PurchaseLineItem, PurchaseLineItemDTO>().ReverseMap();
                c.CreateMap<PurchaseLineItem, PurchaseLineItemVM>().ReverseMap();
                c.CreateMap<PurchaseLineItemVM, PurchaseLineItemDTO>().ReverseMap();

                c.CreateMap<PurchaseLineItemCurrency, PurchaseLineItemCurrencyDTO>().ReverseMap();
                c.CreateMap<PurchaseLineItemCurrency, PurchaseLineItemCurrencyVM>().ReverseMap();
                c.CreateMap<PurchaseLineItemCurrencyVM, PurchaseLineItemCurrencyDTO>().ReverseMap();

                c.CreateMap<Room, RoomDTO>().ReverseMap();
                c.CreateMap<Room, RoomVM>().ReverseMap();
                c.CreateMap<RoomVM, RoomDTO>().ReverseMap();
                c.CreateMap<RoomUpdateRequest, Room>();

                c.CreateMap<RoomGenre, RoomGenreDTO>().ReverseMap();
                c.CreateMap<RoomGenre, RoomGenreVM>().ReverseMap();
                c.CreateMap<RoomGenreVM, RoomGenreDTO>().ReverseMap();

                c.CreateMap<Follow, FollowDTO>().ReverseMap();
                c.CreateMap<Follow, FollowVM>().ReverseMap();
                c.CreateMap<FollowVM, FollowDTO>().ReverseMap();
                c.CreateMap<FollowCreateRequest, Follow>();

                c.CreateMap<Song, SongDTO>().ReverseMap();
                c.CreateMap<Song, SongVM>().ReverseMap()
                    .ForMember(s => s.SongRequests, opt => opt.Ignore())
                    .ReverseMap()
                    .ForMember(s => s.SongRequests, opt => opt.Ignore());
                c.CreateMap<SongVM, SongDTO>().ReverseMap();
                c.CreateMap<SongCreateRequest, Song>();
                c.CreateMap<SongUpdateRequest, Song>();

                c.CreateMap<SongPlaylist, SongPlaylistDTO>().ReverseMap();
                c.CreateMap<SongPlaylist, SongPlaylistVM>().ReverseMap();
                c.CreateMap<SongPlaylistVM, SongPlaylistDTO>().ReverseMap();
                c.CreateMap<SongPlaylistCreateRequest, SongPlaylist>();

                c.CreateMap<Genre, GenreDTO>().ReverseMap();
                c.CreateMap<Genre, GenreVM>().ReverseMap();
                c.CreateMap<GenreVM, GenreDTO>().ReverseMap();

                c.CreateMap<PlaylistGenre, PlaylistGenreDTO>().ReverseMap();
                c.CreateMap<PlaylistGenre, PlaylistGenreVM>().ReverseMap();
                c.CreateMap<PlaylistGenreVM, PlaylistGenreDTO>().ReverseMap();
                c.CreateMap<PlaylistGenreCreateRequest, PlaylistGenre>();

                c.CreateMap<SongQueued, SongQueuedDTO>().ReverseMap();
                c.CreateMap<SongQueued, SongQueuedVM>().ReverseMap();
                c.CreateMap<SongQueuedVM, SongQueuedDTO>().ReverseMap();
                c.CreateMap<SongQueuedCreateRequest, SongQueued>();

                c.CreateMap<SongRequest, SongRequestDTO>().ReverseMap();
                c.CreateMap<SongRequest, SongRequestVM>().ReverseMap();
                c.CreateMap<SongRequestVM, SongRequestDTO>().ReverseMap();
                c.CreateMap<SongRequestCreateRequest, SongRequest>();

                c.CreateMap<Transaction, TransactionDTO>().ReverseMap();
                c.CreateMap<Transaction, TransactionVM>().ReverseMap();
                c.CreateMap<TransactionVM, TransactionDTO>().ReverseMap();
                c.CreateMap<TransactionCreateRequest, Transaction>();

                c.CreateMap<TransactionSongQueued, TransactionSongQueuedDTO>().ReverseMap();
                c.CreateMap<TransactionSongQueued, TransactionSongQueuedVM>().ReverseMap();
                c.CreateMap<TransactionSongQueuedVM, TransactionSongQueuedDTO>().ReverseMap();
                c.CreateMap<TransactionSongQueuedCreateRequest, TransactionSongQueued>();

                c.CreateMap<LogAPI, LogAPIDTO>().ReverseMap();
                c.CreateMap<LogAPI, LogAPIVM>().ReverseMap();
                c.CreateMap<LogAPIVM, LogAPIDTO>().ReverseMap();
                c.CreateMap<LogAPICreateRequest, LogAPI>();

                c.CreateMap<LogError, LogErrorDTO>().ReverseMap();
                c.CreateMap<LogError, LogErrorVM>().ReverseMap();
                c.CreateMap<LogErrorVM, LogErrorDTO>().ReverseMap();
                c.CreateMap<LogErrorCreateRequest, LogError>();
            }).CreateMapper();
        }
    }
}
