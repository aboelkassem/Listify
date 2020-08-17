using AutoMapper;
using Listify.Domain.CodeFirst;
using Listify.Domain.Lib.Entities;
using Listify.Domain.Lib.Requests;
using Listify.Domain.Lib.VMs;
using Listify.Lib.DTOs;
using Listify.Lib.Requests;
using Listify.Lib.VMs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.DAL
{
    public class ListifyServices : IListifyServices
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public ListifyServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<ApplicationUserVM> ReadApplicationUserAsync(Guid id)
        {
            var entity = await _context.ApplicationUsers
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserVM> ReadApplicationUserAsync(string aspNetUserId)
        {
            var entity = await _context.ApplicationUsers
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.AspNetUserId == aspNetUserId && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserVM> CreateApplicationUserAsync(ApplicationUserCreateRequest request)
        {
            var entity = _mapper.Map<ApplicationUser>(request);

            _context.ApplicationUsers.Add(entity);

            _context.Rooms.Add(new Room
            {
                ApplicationUser = entity,
                RoomCode = request.Username
            });

            if (await _context.SaveChangesAsync() > 0)
            {
                return await ReadApplicationUserAsync(entity.Id);
            }

            return null;
        }
        public virtual async Task<ApplicationUserVM> UpdateApplicationUserAsync(ApplicationUserUpdateRequest request)
        {
            var entity = await _context.ApplicationUsers
                .FirstOrDefaultAsync(s => s.Id == request.Id);

            if (entity != null)
            {
                entity.Username = request.Username;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadApplicationUserAsync(request.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteApplicationUserAsync(Guid id)
        {
            var entity = await _context.ApplicationUsers
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<ApplicationUserRoomVM> ReadApplicationUserRoomAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRooms
                .Include(s => s.ApplicationUser)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);
            var vm = _mapper.Map<ApplicationUserRoomVM>(entity);
            return entity != null ? vm : null;
        }
        public virtual async Task<ApplicationUserRoomVM> ReadApplicationUserRoomAsync(Guid applicationUserId, Guid roomId)
        {
            var entity = await _context.ApplicationUsersRooms
                .Include(s => s.ApplicationUser)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.ApplicationUserId == applicationUserId &&
                s.RoomId == roomId && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserRoomVM> CreateApplicationUserRoomAsync(ApplicationUserRoomCreateRequest request, Guid applicationUserId)
        {
            var entity = _mapper.Map<ApplicationUserRoom>(request);

            entity.ApplicationUserId = applicationUserId;
            _context.ApplicationUsersRooms.Add(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadApplicationUserRoomAsync(entity.Id) : null;
        }
        public virtual async Task<ApplicationUserRoomVM> UpdateApplicationUserRoomAsync(ApplicationUserRoomUpdateRequest request)
        {
            var entity = await _context.ApplicationUsersRooms
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active);

            if (entity != null)
            {
                entity.IsOnline = request.IsOnline;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadApplicationUserRoomAsync(entity.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteApplicationUserRoomAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRooms
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<ApplicationUserRoomCurrencyVM> ReadApplicationUserRoomCurrencyAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRoomsCurrencies
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomCurrencyVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserRoomCurrencyVM> CreateApplicationUserRoomCurrencyAsync(ApplicationUserRoomCurrencyCreateRequest request)
        {
            var entity = _mapper.Map<ApplicationUserRoomCurrency>(request);

            _context.ApplicationUsersRoomsCurrencies.Add(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadApplicationUserRoomCurrencyAsync(entity.Id) : null;
        }
        public virtual async Task<ApplicationUserRoomCurrencyVM> UpdateApplicationUserRoomCurrencyAsync(ApplicationUserRoomCurrencyUpdateRequest request)
        {
            var entity = await _context.ApplicationUsersRoomsCurrencies
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active);

            if (entity != null)
            {
                entity.Quantity = request.QuantityToChange;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadApplicationUserRoomCurrencyAsync(entity.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteApplicationUserRoomCurrencyAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRoomsCurrencies
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<ChatMessageVM> ReadChatMessageAsync(Guid id)
        {
            var entity = await _context.ChatMessages
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<ChatMessageVM>(entity) : null;
        }
        public virtual async Task<ChatMessageVM> CreateChatMessageAsync(ChatMessageCreateRequest request)
        {
            var entity = _mapper.Map<ChatMessage>(request);

            _context.ChatMessages.Add(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadChatMessageAsync(entity.Id) : null;
        }
        public virtual async Task<bool> DeleteChatMessageAsync(Guid id)
        {
            var entity = await _context.ChatMessages
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<ApplicationUserRoomConnectionVM> ReadApplicationUserRoomConnectionAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRoomsConnections
                .Include(s => s.ApplicationUserRoom)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomConnectionVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserRoomConnectionVM> ReadApplicationUserRoomConnectionAsync(string connectionId)
        {
            var entity = await _context.ApplicationUsersRoomsConnections
                .Include(s => s.ApplicationUserRoom)
                .FirstOrDefaultAsync(s => s.ConnectionId == connectionId && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomConnectionVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserRoomConnectionVM> CreateApplicationUserRoomConnectionAsync(ApplicationUserRoomConnectionCreateRequest request)
        {
            var entity = _mapper.Map<ApplicationUserRoomConnection>(request);

            _context.ApplicationUsersRoomsConnections.Add(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadApplicationUserRoomConnectionAsync(entity.ConnectionId) : null;
        }
        public virtual async Task<ApplicationUserRoomConnectionVM> UpdateApplicationUserRoomConnectionAsync(ApplicationUserRoomConnectionUpdateRequest request)
        {
            var entity = await _context.ApplicationUsersRoomsConnections
                .FirstOrDefaultAsync(s => s.Id == request.Id);

            if (entity != null)
            {
                entity.IsOnline = request.IsOnline;
                entity.HasPingBeenSent = request.HasPingBeenSent;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadApplicationUserRoomConnectionAsync(entity.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteApplicationUserRoomConnectionAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRoomsConnections
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<CurrencyVM> ReadCurrencyAsync(Guid id)
        {
            var entity = await _context.Currencies
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<CurrencyVM>(entity) : null;
        }
        public virtual async Task<CurrencyVM> CreateCurrencyAsync(CurrencyCreateRequest request)
        {
            var entity = _mapper.Map<Currency>(request);

            _context.Currencies.Add(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadCurrencyAsync(entity.Id) : null;
        }
        public virtual async Task<CurrencyVM> UpdateCurrencyAsync(CurrencyUpdateRequest request)
        {
            var entity = await _context.Currencies
                .FirstOrDefaultAsync(s => s.Id == request.Id);

            if (entity != null)
            {
                entity.CurrencyName = request.CurrencyName;
                entity.Weight = request.Weight;
                entity.QuantityIncreasePerTick = request.QuantityIncreasePerTick;
                entity.TimeSecBetweenTick = request.TimeSecBetweenTick;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadCurrencyAsync(entity.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteCurrencyAsync(Guid id)
        {
            var entity = await _context.Currencies
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<PlaylistVM> ReadPlaylistAsync(Guid id)
        {
            var entity = await _context.Playlists
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<PlaylistVM>(entity) : null;
        }
        public virtual async Task<PlaylistVM> CreatePlaylistAsync(PlaylistCreateRequest request, Guid applicationUserId)
        {
            var user = await _context.ApplicationUsers
                .Include(s => s.Playlists)
                .FirstOrDefaultAsync(s => s.Id == applicationUserId);

            if (user.Playlists.Count < user.PlaylistCountMax)
            {
                var entity = _mapper.Map<Playlist>(request);

                entity.ApplicationUserId = applicationUserId;
                _context.Playlists.Add(entity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadPlaylistAsync(entity.Id);
                }
            }

            return null;
        }
        public virtual async Task<PlaylistVM> UpdatePlaylistAsync(PlaylistUpdateRequest request)
        {
            var entity = await _context.Playlists
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active);

            if (entity != null)
            {
                entity.PlaylistName = request.PlaylistName;
                entity.IsSelected = request.IsSelected;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadPlaylistAsync(entity.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeletePlaylistAsync(Guid id)
        {
            var entity = await _context.Playlists
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<SongVM> ReadSongAsync(Guid id)
        {
            var entity = await _context.Songs
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<SongVM>(entity) : null;
        }
        public virtual async Task<SongVM> CreateSongAsync(SongCreateRequest request)
        {

            var entity = _mapper.Map<Song>(request);

            _context.Songs.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return await ReadSongAsync(entity.Id);
            }

            return null;
        }
        public virtual async Task<SongVM> UpdateSongAsync(SongUpdateRequest request)
        {
            var entity = await _context.Songs
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active);

            if (entity != null)
            {
                entity.SongName = request.SongName;
                entity.SongLengthSeconds = request.SongLengthSeconds;
                entity.YoutubeId = request.YoutubeId;

                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadSongAsync(entity.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteSongAsync(Guid id)
        {
            var entity = await _context.Songs
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<SongPlaylistVM> ReadSongPlaylistAsync(Guid id)
        {
            var entity = await _context.SongsPlaylists
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<SongPlaylistVM>(entity) : null;
        }
        public virtual async Task<SongPlaylistVM> CreateSongPlaylistAsync(SongPlaylistCreateRequest request)
        {

            var entity = _mapper.Map<SongPlaylist>(request);

            _context.SongsPlaylists.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return await ReadSongPlaylistAsync(entity.Id);
            }

            return null;
        }
        public virtual async Task<bool> DeleteSongPlaylistAsync(Guid id)
        {
            var entity = await _context.SongsPlaylists
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<SongQueuedVM> ReadSongQueuedAsync(Guid id)
        {
            var entity = await _context.SongsQueued
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<SongQueuedVM>(entity) : null;
        }
        public virtual async Task<SongQueuedVM> CreateSongQueuedAsync(SongQueuedCreateRequest request)
        {

            var entity = _mapper.Map<SongQueued>(request);

            _context.SongsQueued.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return await ReadSongQueuedAsync(entity.Id);
            }

            return null;
        }
        public virtual async Task<bool> DeleteSongQueuedAsync(Guid id)
        {
            var entity = await _context.SongsQueued
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<TransactionVM> ReadTransactionAsync(Guid id)
        {
            var entity = await _context.Transactions
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<TransactionVM>(entity) : null;
        }
        public virtual async Task<TransactionVM> CreateTransactionAsync(TransactionCreateRequest request)
        {

            var entity = _mapper.Map<Transaction>(request);

            _context.Transactions.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return await ReadTransactionAsync(entity.Id);
            }

            return null;
        }
        public virtual async Task<bool> DeleteTransactionAsync(Guid id)
        {
            var entity = await _context.Transactions
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<TransactionSongQueuedVM> ReadTransactionSongQueuedAsync(Guid id)
        {
            var entity = await _context.TransactionsSongsQueued
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<TransactionSongQueuedVM>(entity) : null;
        }
        public virtual async Task<TransactionSongQueuedVM> CreateTransactionSongQueuedAsync(TransactionSongQueuedCreateRequest request)
        {

            var entity = _mapper.Map<TransactionSongQueued>(request);

            _context.TransactionsSongsQueued.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return await ReadTransactionSongQueuedAsync(entity.Id);
            }

            return null;
        }
        public virtual async Task<bool> DeleteTransactionSongQueuedAsync(Guid id)
        {
            var entity = await _context.TransactionsSongsQueued
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<LogAPIVM> ReadLogAPIAsync(Guid id)
        {
            var entity = await _context.LogsAPI
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<LogAPIVM>(entity) : null;
        }
        public virtual async Task<LogAPIVM> CreateLogAPIAsync(LogAPICreateRequest request, Guid applicationUserId)
        {
            var entity = _mapper.Map<LogAPI>(request);

            entity.ApplicationUserId = applicationUserId;
            _context.LogsAPI.Add(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadLogAPIAsync(entity.Id) : null;
        }

        public virtual async Task<LogErrorVM> ReadLogErrorAsync(Guid id)
        {
            var entity = await _context.LogsErrors
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<LogErrorVM>(entity) : null;
        }
        public virtual async Task<LogErrorVM> CreateLogErrorAsync(LogErrorCreateRequest request, Guid applicationUserId)
        {
            var entity = _mapper.Map<LogError>(request);

            entity.ApplicationUserId = applicationUserId;
            _context.LogsErrors.Add(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadLogErrorAsync(entity.Id) : null;
        }

        public virtual async Task<RoomDTO[]> ReadRoomsAsync()
        {
            var entities = await _context.Rooms
                        .Where(s => s.IsRoomOnline && s.IsRoomPublic && s.Active)
                        .ToListAsync();

            var dtos = new List<RoomDTO>();

            entities.ForEach(s => dtos.Add(_mapper.Map<RoomDTO>(s)));

            return dtos.ToArray();
        }
        public virtual async Task<RoomVM> ReadRoomAsync(Guid id)
        {
            var entity = await _context.Rooms
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<RoomVM>(entity) : null;
        }
        public virtual async Task<RoomVM> ReadRoomAsync(string roomCode)
        {
            var entity = await _context.Rooms
                .FirstOrDefaultAsync(s => s.RoomCode.Trim().ToLower() == roomCode.Trim().ToLower() && s.Active);

            return entity != null ? _mapper.Map<RoomVM>(entity) : null;
        }
        public virtual async Task<RoomVM> UpdateRoomAsync(RoomUpdateRequest request)
        {
            var entity = await _context.Rooms
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active);

            if (entity != null)
            {
                entity.RoomCode = request.RoomCode;
                entity.IsRoomPublic = request.IsRoomPublic;
                entity.IsRoomOnline = request.IsRoomOnline;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadRoomAsync(request.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteRoomAsync(Guid id)
        {
            var entity = await _context.Rooms
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual async Task<V> CreateAsync<T, U, V>(T request)
            where T : BaseRequest
            where U : BaseEntity
            where V : BaseVM
        {
            // Converting Request To Entity
            var entity = _mapper.Map<U>(request);

            // Add Entity
            _context.Add(entity);


            // Returning The ViewModel of the entity after saving on DB
            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<V>(entity) : null;
        }
        protected virtual async Task<V> ReadAsync<U, V>(Guid id)
            where U : BaseEntity
            where V : BaseVM
        {
            // get the entity of type U
            var entity = await _context.FindAsync<U>(id);

            // Returning The ViewModel of the entity
            return entity != null ? _mapper.Map<V>(entity) : null;
        }
        protected virtual async Task<V> UpdateAsync<T, U, V>(T request)
            where T : BaseUpdateRequest
            where U : BaseEntity
            where V : BaseVM
        {
            // get the entity of type U
            var entity = await _context.FindAsync<U>(request.Id);

            if (entity != null)
            {
                _mapper.Map(request, entity);
                _context.Entry(entity).State = EntityState.Modified;

                // Returning The ViewModel of the entity after saving on DB
                if (await _context.SaveChangesAsync() > 0)
                    return _mapper.Map<V>(entity);
            }

            return null;
        }
        protected virtual async Task<bool> DeleteAsync<U>(Guid id)
            where U : BaseEntity
        {
            // get the entity of type U
            var entity = await _context.FindAsync<U>(id);

            if (entity != null)
            {
                entity.Active = false;
                _context.Entry(entity).State = EntityState.Modified;
                //// Returning true if deleted successfully
                //_context.Remove(entity);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual void Dispose()
        {
        }
    }
}
