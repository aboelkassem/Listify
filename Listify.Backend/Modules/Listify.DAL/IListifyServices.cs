﻿using Listify.Domain.Lib.Enums;
using Listify.Lib.DTOs;
using Listify.Lib.Requests;
using Listify.Lib.Responses;
using Listify.Lib.VMs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Listify.DAL
{
    public interface IListifyServices : IDisposable
    {
        Task<ApplicationUserVM> CreateApplicationUserAsync(ApplicationUserCreateRequest request);
        Task<ApplicationUserRoomVM> CreateApplicationUserRoomAsync(ApplicationUserRoomCreateRequest request, Guid applicationUserId);
        Task<ApplicationUserRoomConnectionVM> CreateApplicationUserRoomConnectionAsync(ApplicationUserRoomConnectionCreateRequest request);
        Task<ApplicationUserRoomCurrencyVM> CreateApplicationUserRoomCurrencyAsync(ApplicationUserRoomCurrencyCreateRequest request);
        Task<ChatMessageVM> CreateChatMessageAsync(ChatMessageCreateRequest request);
        Task<CurrencyVM> CreateCurrencyAsync(CurrencyCreateRequest request, Guid applicationUserId);
        Task<LogAPIVM> CreateLogAPIAsync(LogAPICreateRequest request, Guid applicationUserId);
        Task<LogErrorVM> CreateLogErrorAsync(LogErrorCreateRequest request, Guid applicationUserId);
        Task<PlaylistVM> CreatePlaylistAsync(PlaylistCreateRequest request, Guid applicationUserId);
        Task<SongVM> CreateSongAsync(SongCreateRequest request);
        Task<SongPlaylistVM> CreateSongPlaylistAsync(SongPlaylistCreateRequest request, Guid applicationUserId);
        Task<SongQueuedVM> CreateSongQueuedAsync(SongQueuedCreateRequest request);
        Task<TransactionVM> CreateTransactionAsync(TransactionCreateRequest request);
        Task<TransactionSongQueuedVM> CreateTransactionSongQueuedAsync(TransactionSongQueuedCreateRequest request);
        Task<bool> DeleteApplicationUserAsync(Guid id, Guid applicationUserId);
        Task<bool> DeleteApplicationUserRoomAsync(Guid id);
        Task<bool> DeleteApplicationUserRoomConnectionAsync(Guid id);
        Task<bool> DeleteApplicationUserRoomCurrencyAsync(Guid id);
        Task<bool> DeleteChatMessageAsync(Guid id);
        Task<bool> DeleteCurrencyAsync(Guid id);
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
        Task<ApplicationUserRoomCurrencyVM> ReadApplicationUserRoomCurrencyAsync(Guid id);
        Task<ApplicationUserRoomCurrencyVM[]> ReadApplicationUserRoomCurrenciesRoomAsync(Guid applicationUserRoomId);
        Task<ApplicationUserRoomCurrencyVM> ReadApplicationUserRoomCurrencyAsync(Guid applicationUserRoomId, Guid currencyId);
        Task<ChatMessageVM> ReadChatMessageAsync(Guid id);
        Task<CurrencyVM> ReadCurrencyAsync(Guid id);
        Task<LogAPIVM> ReadLogAPIAsync(Guid id);
        Task<LogErrorVM> ReadLogErrorAsync(Guid id);
        Task<PlaylistVM> ReadPlaylistAsync(Guid id, Guid applicationUserId);
        Task<PlaylistDTO[]> ReadPlaylistsAsync(Guid applicationUserId);
        Task<CurrencyDTO[]> ReadCurrenciesAsync();
        Task<RoomVM> ReadRoomAsync(string roomCode);
        Task<RoomDTO[]> ReadRoomsAsync();
        Task<RoomVM> ReadRoomAsync(Guid id);
        Task<SongVM> ReadSongAsync(Guid id);
        Task<SongVM> ReadSongAsync(string videoId);
        Task<SongPlaylistVM[]> ReadSongsPlaylistAsync(Guid playlistId);
        Task<SongPlaylistVM> ReadSongPlaylistAsync(Guid id);
        Task<SongQueuedVM> ReadSongQueuedAsync(Guid id);
        Task<SongQueuedVM> DequeueSongQueuedAsync(Guid roomId, Guid applicationUserId);
        Task<SongQueuedVM> QueueSongPlaylistNext(Guid applicationUserId);
        Task<SongQueuedVM[]> ReadSongsQueuedAsync(Guid roomId);
        Task<TransactionVM> ReadTransactionAsync(Guid id);
        Task<TransactionSongQueuedVM> ReadTransactionSongQueuedAsync(Guid id);
        Task<ApplicationUserVM> UpdateApplicationUserAsync(ApplicationUserUpdateRequest request, Guid applicationUserId);
        Task<ApplicationUserRoomVM> UpdateApplicationUserRoomAsync(ApplicationUserRoomUpdateRequest request);
        Task<ApplicationUserRoomCurrencyVM> UpdateApplicationUserRoomCurrencyAsync(ApplicationUserRoomCurrencyUpdateRequest request);
        Task<ApplicationUserRoomConnectionVM> UpdateApplicationUserRoomConnectionAsync(ApplicationUserRoomConnectionUpdateRequest request);
        Task<CurrencyVM> UpdateCurrencyAsync(CurrencyCreateRequest request, Guid applicationUserId);
        Task<PlaylistVM> UpdatePlaylistAsync(PlaylistCreateRequest request, Guid applicationUserId);
        Task<RoomVM> UpdateRoomAsync(RoomUpdateRequest request);
        Task<SongVM> UpdateSongAsync(SongUpdateRequest request);

        Task<YoutubeResults> SearchYoutubeLightAsync(string searchSnippet);
        Task<YoutubeResults> SearchYoutubeAsync(string searchSnippet);
        Task<ICollection<ApplicationUserRoomCurrencyVM>> AddCurrencyQuantityToAllUsersInRoomAsync(Guid roomId, Guid currencyId, int currencyQuantity, TransactionType transactionType);
        Task<ICollection<ApplicationUserRoomConnectionVM>> PingApplicationUsersRoomsConnections();
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
    }
}