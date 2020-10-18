using Listify.Domain.Lib.Enums;
using Listify.Lib.DTOs;
using Listify.Lib.Requests;
using Listify.Lib.Responses;
using Listify.Lib.VMs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Listify.DAL
{
    public interface IListifyDAL : IDisposable
    {
        Task<ApplicationUserVM> CreateApplicationUserAsync(ApplicationUserCreateRequest request);
        Task<ApplicationUserRoomVM> CreateApplicationUserRoomAsync(ApplicationUserRoomCreateRequest request, Guid applicationUserId);
        Task<ApplicationUserRoomConnectionVM> CreateApplicationUserRoomConnectionAsync(ApplicationUserRoomConnectionCreateRequest request);
        Task<ApplicationUserRoomCurrencyRoomVM> CreateApplicationUserRoomCurrencyRoomAsync(ApplicationUserRoomCurrencyRoomCreateRequest request);
        Task<ApplicationUserRoomCurrencyRoomVM[]> CheckApplicationUserRoomCurrenciesRoomAsync(Guid applicationUserRoomId);
        Task<bool> UpdateApplicationUserRoomAndRoomToOfflineAsync(Guid applicationUserRoomId);
        Task<ChatMessageVM> CreateChatMessageAsync(ChatMessageCreateRequest request);
        Task<LogAPIVM> CreateLogAPIAsync(LogAPICreateRequest request, Guid applicationUserId);
        Task<LogErrorVM> CreateLogErrorAsync(LogErrorCreateRequest request, Guid applicationUserId);
        Task<PlaylistVM> CreatePlaylistAsync(PlaylistCreateRequest request, Guid applicationUserId);
        Task<SongVM> CreateSongAsync(SongCreateRequest request);
        Task<SongVM> CreateSongAsync(string youtubeId);
        Task<SongVM> CreateSongAsync(YoutubeResults.YoutubeResult songSearchResult);
        Task<SongVM> CreateSongAsync(Google.Apis.YouTube.v3.Data.PlaylistItem playlistItem);
        Task<SongVM> CreateSongAsync(Google.Apis.YouTube.v3.Data.SearchResult songSearchResult);
        Task<SongPlaylistVM> CreateSongPlaylistAsync(SongPlaylistCreateRequest request, Guid applicationUserId);
        Task<SongQueuedVM> CreateSongQueuedAsync(SongQueuedCreateRequest request);
        Task<TransactionVM> CreateTransactionAsync(TransactionCreateRequest request);
        Task<TransactionSongQueuedVM> CreateTransactionSongQueuedAsync(TransactionSongQueuedCreateRequest request);
        Task<bool> DeleteApplicationUserAsync(Guid id, Guid applicationUserId);
        Task<bool> DeleteApplicationUserRoomAsync(Guid id);
        Task<bool> DeleteApplicationUserRoomConnectionAsync(Guid id);
        Task<bool> DeleteApplicationUserRoomCurrencyRoomAsync(Guid id);
        Task<bool> DeleteChatMessageAsync(Guid id);
        Task<bool> DeletePlaylistAsync(Guid id, Guid applicationUserId);
        Task<bool> DeleteRoomAsync(Guid id);
        Task<bool> DeleteSongAsync(Guid id);
        Task<bool> DeleteSongPlaylistAsync(Guid id, Guid applicationUserId);
        Task<bool> DeleteSongQueuedAsync(Guid id);
        Task<bool> DeleteTransactionAsync(Guid id);
        Task<bool> DeleteTransactionSongQueuedAsync(Guid id);
        Task<ApplicationUserVM> ReadApplicationUserAsync(Guid id);
        Task<ApplicationUserVM> ReadApplicationUserAsync(string aspNetUserId);
        Task<ApplicationUserRoomVM> ReadApplicationUserRoomAsync(Guid id);
        Task<ApplicationUserRoomVM> ReadApplicationUserRoomOwnerAsync(Guid roomId);
        Task<ApplicationUserRoomVM> ReadApplicationUserRoomAsync(Guid applicationUserId, Guid roomId);
        Task<ApplicationUserRoomConnectionVM[]> ReadApplicationUsersRoomsConnectionsAsync(Guid roomId);
        Task<ApplicationUserRoomConnectionVM[]> ReadApplicationUserRoomConnectionByApplicationUserRoomIdAsync(Guid applicationUserRoomId);
        Task<ApplicationUserRoomConnectionVM> ReadApplicationUserRoomConnectionAsync(Guid id);
        Task<ApplicationUserRoomConnectionVM> ReadApplicationUserRoomConnectionAsync(string connectionId);
        Task<ApplicationUserRoomCurrencyRoomVM> ReadApplicationUserRoomCurrencyRoomAsync(Guid id);
        Task<ApplicationUserRoomCurrencyRoomVM> ReadApplicationUserRoomCurrencyRoomAsync(Guid applicationUserRoomId, Guid currencyRoomId);
        Task<ChatMessageVM> ReadChatMessageAsync(Guid id);
        Task<CurrencyVM> ReadCurrencyAsync(Guid id);
        Task<LogAPIVM> ReadLogAPIAsync(Guid id);
        Task<LogErrorVM> ReadLogErrorAsync(Guid id);
        Task<PlaylistVM> ReadPlaylistAsync(Guid id, Guid applicationUserId);
        Task<PlaylistVM[]> ReadPlaylistsAsync(Guid applicationUserId);
        Task<PlaylistVM> ReadPlaylistSelectedAsync(Guid applicationUserId); // 1 references
        Task<SongPlaylistVM[]> AddSongsToPlaylistAsync(SongVM[] songs, Guid playlistId, Guid applicationUserId); // 1 references
        Task<PlaylistVM[]> ReadPlaylistsCommunityAsync();
        Task<RoomVM> ReadRoomAsync(string roomCode);
        Task<RoomVM[]> ReadRoomsAsync();
        Task<RoomVM[]> ReadRoomsFollowsAsync(Guid applicationUserId);
        Task<RoomVM> ReadRoomAsync(Guid id);
        Task<SongVM> ReadSongAsync(Guid id);
        Task<SongVM> ReadSongAsync(string youtubeId);
        Task<SongPlaylistVM[]> ReadSongsPlaylistAsync(Guid playlistId);
        Task<SongPlaylistVM> ReadSongPlaylistAsync(Guid id);
        Task<SongQueuedVM> ReadSongQueuedAsync(Guid id);
        Task<SongQueuedVM> DequeueSongQueuedAsync(Guid roomId, Guid applicationUserId);
        Task<ApplicationUserRoomCurrencyRoomVM[]> SkipSongAsync(Guid songQueuedId);
        Task<SongQueuedVM> QueueSongPlaylistNext(Guid applicationUserId);
        Task<SongQueuedVM[]> ReadSongsQueuedAsync(Guid roomId);
        Task<TransactionVM> ReadTransactionAsync(Guid id);
        Task<TransactionSongQueuedVM> ReadTransactionSongQueuedAsync(Guid id);
        Task<ApplicationUserVM> UpdateApplicationUserAsync(ApplicationUserUpdateRequest request, Guid applicationUserId);
        Task<ApplicationUserRoomVM> UpdateApplicationUserRoomAsync(ApplicationUserRoomUpdateRequest request);
        Task<ApplicationUserRoomConnectionVM> UpdateApplicationUserRoomConnectionAsync(ApplicationUserRoomConnectionUpdateRequest request);
        Task<PlaylistVM> UpdatePlaylistAsync(PlaylistCreateRequest request, Guid applicationUserId);
        Task<RoomVM> UpdateRoomAsync(RoomUpdateRequest request);
        Task<SongVM> UpdateSongAsync(SongUpdateRequest request);
        Task<SongVM> UpdateSongAsync(string youtubeId);

        Task<YoutubeResults> SearchYoutubeLightAsync(string searchSnippet);
        Task<SongVM> SearchYoutubeAndReturnFirstResultAsync(string searchSnippet);
        Task<YoutubeResults> SearchYoutubeAsync(string searchSnippet);
        Task<int> GetTimeOfYoutubeVideoBySeconds(string youtubeId);
        Task<ICollection<ApplicationUserRoomCurrencyRoomVM>> AddCurrencyQuantityToAllUsersInRoomAsync(Guid roomId, Guid currencyRoomId, int currencyQuantity, TransactionType transactionType);
        Task<ICollection<ApplicationUserRoomConnectionVM>> PingApplicationUsersRoomsConnectionsAsync();
        Task RestartSongPlaylistCountAsync(Guid applicationUserId);
        Task<bool> WagerQuantitySongQueued(WagerQuantitySongQueuedRquest request);

        Task<ICollection<PurchaseDTO>> ReadPurchasesAsync();
        Task<ICollection<PurchaseDTO>> ReadPurchasesAsync(Guid applicationUserId);
        Task<PurchaseVM> ReadPurchaseAsync(Guid id, Guid applicationUserId);
        Task<PurchaseVM> CreatePurchaseAsync(PurchaseCreateRequest request, Guid applicationUserId);

        Task<ICollection<PurchasableItemDTO>> ReadPurchasableItemsAsync();
        Task<PurchasableItemVM> ReadPurchasableItemAsync(Guid id);
        Task<PurchasableItemVM> CreatePurchasableItemAsync(PurchasableItemCreateRequest request);
        Task<PurchasableItemVM> UpdatePurchasableItemAsync(PurchasableItemCreateRequest request);
        Task<bool> DeletePurchasableItemAsync(Guid id, Guid applicationUserId);

        Task<CurrencyRoomVM[]> ReadCurrenciesRoomAsync(Guid roomId);
        Task<CurrencyRoomVM[]> CheckCurrenciesRoomAsync(Guid roomId);
        Task<CurrencyRoomVM> ReadCurrencyRoomAsync(Guid id);
        Task<CurrencyRoomVM> CreateCurrencyRoomAsync(CurrencyRoomCreateRequest request, Guid applicationUserId);
        Task<CurrencyRoomVM> UpdateCurrencyRoomAsync(CurrencyRoomCreateRequest request, Guid applicationUserId);
        Task<bool> DeleteCurrencyRoomAsync(Guid id);

        Task<bool> CheckAuthToLockedRoomAsync(string roomCode, Guid roomId);
        Task<bool> IsUsernameAvailableAsync(string applicationUsername, Guid applicationUserId);
        Task<bool> IsRoomCodeAvailableAsync(string roomCode, Guid applicationUserId);

        Task<SongQueuedVM[]> QueuePlaylistInRoomHomeAsync(Guid playlistId, bool isRandomized, Guid applicationUserId);
        Task<SongPlaylistVM[]> AddYoutubePlaylistToPlaylistAsync(string youtubePlaylistUrl, Guid playlistId, Guid applicationUserId);

        Task<bool> ClearSongsQueuedAsync(Guid roomId);

        //Task<bool> UpvoteSongQueuedNoWager(Guid songQueuedId);
        //Task<bool> DownvoteSongQueuedNoWager(Guid songQueuedId);

        Task<GenreDTO[]> ReadGenresAsync();

        Task<FollowVM[]> ReadFollowsByRoomIdAsync(Guid roomId);
        Task<FollowVM[]> ReadFollowsByApplicationUserIdAsync(Guid applicationUserId);
        Task<FollowVM> ReadFollowAsync(Guid id);
        Task<FollowVM> ReadFollowAsync(Guid roomId, Guid applicationUserId);
        Task<FollowVM> CreateFollowAsync(FollowCreateRequest request, Guid applicationUserId);
        Task<bool> DeleteFollowAsync(Guid followId, Guid applicationUserId);

        Task<bool> ClearUserProfileImageAsync(Guid applicationUserId);
        Task<bool> ClearRoomImageAsync(Guid roomId, Guid applicationUserId);
        Task<bool> ClearPlaylistImageAsync(Guid playlistId, Guid applicationUserId);

        Task<ProfileVM> ReadProfileAsync(string username);
        Task<ProfileVM> UpdateProfileAsync(ProfileUpdateRequest request, Guid applicationUserId);
        Task<ApplicationUserVM> UpdateApplicationUserProfileImageUrlAsync(string profileImageUrl, Guid profileId, Guid applicationUserId);
        Task<ApplicationUserVM> UpdateApplicationUserRoomImageUrlAsync(string roomImageUrl, Guid roomId, Guid applicationUserId);
        Task<ApplicationUserVM> UpdatePlaylistImageUrlAsync(string playlistImageUrl, Guid playlistId, Guid applicationUserId);
        Task<ApplicationUserRoomVM[]> ReadApplicationUsersRoomOnlineAsync(string connectionId);
    }
}