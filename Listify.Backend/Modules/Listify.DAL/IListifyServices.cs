﻿using Listify.Lib.Requests;
using Listify.Lib.VMs;
using System;
using System.Threading.Tasks;

namespace Listify.DAL
{
    public interface IListifyServices
    {
        Task<ApplicationUserVM> CreateApplicationUserAsync(ApplicationUserCreateRequest request);
        Task<ApplicationUserRoomVM> CreateApplicationUserRoomAsync(ApplicationUserRoomCreateRequest request, Guid applicationUserId);
        Task<ApplicationUserRoomConnectionVM> CreateApplicationUserRoomConnectionAsync(ApplicationUserRoomConnectionCreateRequest request);
        Task<ApplicationUserRoomCurrencyVM> CreateApplicationUserRoomCurrencyAsync(ApplicationUserRoomCurrencyCreateRequest request);
        Task<ChatMessageVM> CreateChatMessageAsync(ChatMessageCreateRequest request);
        Task<CurrencyVM> CreateCurrencyAsync(CurrencyCreateRequest request);
        Task<LogAPIVM> CreateLogAPIAsync(LogAPICreateRequest request, Guid applicationUserId);
        Task<LogErrorVM> CreateLogErrorAsync(LogErrorCreateRequest request, Guid applicationUserId);
        Task<PlaylistVM> CreatePlaylistAsync(PlaylistCreateRequest request, Guid applicationUserId);
        Task<SongVM> CreateSongAsync(SongCreateRequest request);
        Task<SongPlaylistVM> CreateSongPlaylistAsync(SongPlaylistCreateRequest request);
        Task<SongQueuedVM> CreateSongQueuedAsync(SongQueuedCreateRequest request);
        Task<TransactionVM> CreateTransactionAsync(TransactionCreateRequest request);
        Task<TransactionSongQueuedVM> CreateTransactionSongQueuedAsync(TransactionSongQueuedCreateRequest request);
        Task<bool> DeleteApplicationUserAsync(Guid id);
        Task<bool> DeleteApplicationUserRoomAsync(Guid id);
        Task<bool> DeleteApplicationUserRoomConnectionAsync(Guid id);
        Task<bool> DeleteApplicationUserRoomCurrencyAsync(Guid id);
        Task<bool> DeleteChatMessageAsync(Guid id);
        Task<bool> DeleteCurrencyAsync(Guid id);
        Task<bool> DeletePlaylistAsync(Guid id);
        Task<bool> DeleteRoomAsync(Guid id);
        Task<bool> DeleteSongAsync(Guid id);
        Task<bool> DeleteSongPlaylistAsync(Guid id);
        Task<bool> DeleteSongQueuedAsync(Guid id);
        Task<bool> DeleteTransactionAsync(Guid id);
        Task<bool> DeleteTransactionSongQueuedAsync(Guid id);
        Task<ApplicationUserVM> ReadApplicationUserAsync(Guid id);
        Task<ApplicationUserVM> ReadApplicationUserAsync(string aspNetUserId);
        Task<ApplicationUserRoomVM> ReadApplicationUserRoomAsync(Guid id);
        Task<ApplicationUserRoomConnectionVM> ReadApplicationUserRoomConnectionAsync(Guid id);
        Task<ApplicationUserRoomConnectionVM> ReadApplicationUserRoomConnectionAsync(string connectionId);
        Task<ApplicationUserRoomCurrencyVM> ReadApplicationUserRoomCurrencyAsync(Guid id);
        Task<ChatMessageVM> ReadChatMessageAsync(Guid id);
        Task<CurrencyVM> ReadCurrencyAsync(Guid id);
        Task<LogAPIVM> ReadLogAPIAsync(Guid id);
        Task<LogErrorVM> ReadLogErrorAsync(Guid id);
        Task<PlaylistVM> ReadPlaylistAsync(Guid id);
        Task<RoomVM> ReadRoomAsync(Guid id);
        Task<SongVM> ReadSongAsync(Guid id);
        Task<SongPlaylistVM> ReadSongPlaylistAsync(Guid id);
        Task<SongQueuedVM> ReadSongQueuedAsync(Guid id);
        Task<TransactionVM> ReadTransactionAsync(Guid id);
        Task<TransactionSongQueuedVM> ReadTransactionSongQueuedAsync(Guid id);
        Task<ApplicationUserVM> UpdateApplicationUserAsync(ApplicationUserUpdateRequest request);
        Task<ApplicationUserRoomVM> UpdateApplicationUserRoomAsync(ApplicationUserRoomUpdateRequest request);
        Task<ApplicationUserRoomConnectionVM> UpdateApplicationUserRoomConnectionAsync(ApplicationUserRoomConnectionUpdateRequest request);
        Task<ApplicationUserRoomCurrencyVM> UpdateApplicationUserRoomCurrencyAsync(ApplicationUserRoomCurrencyUpdateRequest request);
        Task<CurrencyVM> UpdateCurrencyAsync(CurrencyUpdateRequest request);
        Task<PlaylistVM> UpdatePlaylistAsync(PlaylistUpdateRequest request);
        Task<RoomVM> UpdateRoomAsync(RoomUpdateRequest request);
        Task<SongVM> UpdateSongAsync(SongUpdateRequest request);
    }
}