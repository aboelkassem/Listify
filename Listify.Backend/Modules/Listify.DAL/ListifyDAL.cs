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
using Listify.Lib.Responses;
using Listify.Domain.Lib.Enums;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Listify.Paths;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

namespace Listify.DAL
{
    public class ListifyDAL : IListifyDAL
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public ListifyDAL(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<ApplicationUserVM> ReadApplicationUserAsync(Guid id)
        {
            var entity = await _context.ApplicationUsers
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity.Room.IsRoomLocked)
            {
                if (entity.Room.RoomKey != null)
                {
                    entity.Room.RoomKey = Encoding.UTF8.GetString(Convert.FromBase64String(entity.Room.RoomKey));
                }
            }

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

            await _context.ApplicationUsers.AddAsync(entity);

            var room = new Room
            {
                ApplicationUser = entity,
                RoomCode = request.Username
            };
            await _context.Rooms.AddAsync(room);

            var applicationUserRoom = new ApplicationUserRoom
            {
                ApplicationUser = entity,
                IsOnline = true,
                IsOwner = true,
                Room = room
            };


            await _context.ApplicationUsersRooms.AddAsync(applicationUserRoom);

            if (await _context.SaveChangesAsync() > 0)
            {
                await CheckCurrenciesRoomAsync(room.Id);
                return await ReadApplicationUserAsync(entity.Id);
            }

            return null;
        }
        public virtual async Task<ApplicationUserVM> UpdateApplicationUserAsync(ApplicationUserUpdateRequest request, Guid applicationUserId)
        {
            var entity = await _context.ApplicationUsers
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Id == applicationUserId && s.Active);

            if (entity != null)
            {
                string encodedRoomKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.RoomKey));
                if (encodedRoomKey == null)
                {
                    encodedRoomKey = "";
                }

                entity.Username = request.Username;
                entity.Room.RoomCode = request.RoomCode;
                entity.Room.RoomTitle = request.RoomTitle;
                entity.Room.RoomKey = encodedRoomKey;
                entity.Room.AllowRequests = request.AllowRequests;
                entity.Room.IsRoomLocked = request.IsRoomLocked;
                entity.Room.IsRoomPublic = request.IsRoomPublic;
                entity.Room.IsRoomOnline = request.IsRoomOnline;
                entity.Room.MatureContent = request.MatureContent;
                entity.Room.MatureContentChat = request.MatureContentChat;
                entity.TimeStamp = DateTime.UtcNow;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadApplicationUserAsync(request.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteApplicationUserAsync(Guid id, Guid applicationUserId)
        {
            var entity = await _context.ApplicationUsers
                .FirstOrDefaultAsync(s => s.Id == id && s.Id == applicationUserId && s.Active);

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
        public virtual async Task<ApplicationUserRoomVM> ReadApplicationUserRoomOwnerAsync(Guid roomId)
        {
            var entity = await _context.ApplicationUsersRooms
                .Include(s => s.ApplicationUser)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.RoomId == roomId && s.IsOwner && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomVM>(entity) : null;
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
            await _context.ApplicationUsersRooms.AddAsync(entity);

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
        public virtual async Task<bool> UpdateApplicationUserRoomAndRoomToOfflineAsync(Guid applicationUserRoomId)
        {
            var applicaitonUserRoom = await _context.ApplicationUsersRooms
                .FirstOrDefaultAsync(s => s.Id == applicationUserRoomId && s.Active && s.IsOnline && s.Room.IsRoomOnline);

            if (applicaitonUserRoom != null)
            {
                applicaitonUserRoom.IsOnline = false;
                applicaitonUserRoom.Room.IsRoomOnline = false;
                _context.Entry(applicaitonUserRoom).State = EntityState.Modified;
                _context.Entry(applicaitonUserRoom.Room).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
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

        public virtual async Task<ApplicationUserRoomCurrencyRoomVM[]> ReadApplicationUserRoomCurrenciesRoomAsync(Guid applicationUserRoomId)
        {
            //await CheckApplicationUserRoomCurrenciesRoomAsync(applicationUserRoomId);

            var entities = await _context.ApplicationUsersRoomsCurrenciesRooms
                .Include(s => s.ApplicationUserRoom)
                .Include(s => s.CurrencyRoom)
                .Include(s => s.CurrencyRoom.Currency)
                .Where(s => s.ApplicationUserRoomId == applicationUserRoomId && s.Active)
                .ToListAsync();

            var vms = new List<ApplicationUserRoomCurrencyRoomVM>();

            entities.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomCurrencyRoomVM>(s)));

            return vms.ToArray();
        }
        public virtual async Task<ApplicationUserRoomCurrencyRoomVM> ReadApplicationUserRoomCurrencyRoomAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRoomsCurrenciesRooms
                .Include(s => s.ApplicationUserRoom)
                .Include(s => s.CurrencyRoom)
                .Include(s => s.CurrencyRoom.Currency)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomCurrencyRoomVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserRoomCurrencyRoomVM> ReadApplicationUserRoomCurrencyRoomAsync(Guid applicationUserRoomId, Guid currencyRoomId)
        {
            await CheckApplicationUserRoomCurrenciesRoomAsync(applicationUserRoomId);

            var entity = await _context.ApplicationUsersRoomsCurrenciesRooms
                .Include(s => s.ApplicationUserRoom)
                .Include(s => s.CurrencyRoom)
                .Include(s => s.CurrencyRoom.Currency)
                .FirstOrDefaultAsync(s => s.ApplicationUserRoomId == applicationUserRoomId &&
                s.CurrencyRoomId == currencyRoomId && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomCurrencyRoomVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserRoomCurrencyRoomVM[]> CheckApplicationUserRoomCurrenciesRoomAsync(Guid applicationUserRoomId)
        {
            var applicationUserRoom = await _context.ApplicationUsersRooms
                .FirstOrDefaultAsync(s => s.Id == applicationUserRoomId && s.Active && s.IsOnline);

            if (applicationUserRoom != null)
            {
                var currenciesRoom = await _context.CurrenciesRooms
                    .Where(s => s.RoomId == applicationUserRoom.RoomId && s.Active)
                    .ToListAsync();

                foreach (var currencyRoom in currenciesRoom)
                {
                    var applicationUserRoomCurrencyRoom = await _context.ApplicationUsersRoomsCurrenciesRooms
                        .FirstOrDefaultAsync(s => s.ApplicationUserRoomId == applicationUserRoomId &&
                        s.CurrencyRoomId == currencyRoom.Id);

                    if (applicationUserRoomCurrencyRoom == null)
                    {
                        await _context.ApplicationUsersRoomsCurrenciesRooms.AddAsync(new ApplicationUserRoomCurrencyRoom
                        {
                            ApplicationUserRoomId = applicationUserRoomId,
                            CurrencyRoomId = currencyRoom.Id,
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return await ReadApplicationUserRoomCurrenciesRoomAsync(applicationUserRoomId);
        }
        public virtual async Task<ApplicationUserRoomCurrencyRoomVM> CreateApplicationUserRoomCurrencyRoomAsync(ApplicationUserRoomCurrencyRoomCreateRequest request)
        {
            var entity = _mapper.Map<ApplicationUserRoomCurrencyRoom>(request);

            await _context.ApplicationUsersRoomsCurrenciesRooms.AddAsync(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadApplicationUserRoomCurrencyRoomAsync(entity.Id) : null;
        }
        public virtual async Task<bool> DeleteApplicationUserRoomCurrencyRoomAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRoomsCurrenciesRooms
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

            await _context.ChatMessages.AddAsync(entity);

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

        public virtual async Task<ApplicationUserRoomConnectionVM[]> ReadApplicationUsersRoomsConnectionsAsync(Guid roomId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var connections = await context.ApplicationUsersRoomsConnections
                    .Include(s => s.ApplicationUserRoom)
                    .Where(s => s.ApplicationUserRoom.RoomId == roomId && s.Active && s.ApplicationUserRoom.Active)
                    .ToListAsync();

                    var vms = new List<ApplicationUserRoomConnectionVM>();
                    connections.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(s)));

                    return vms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public virtual async Task<ApplicationUserRoomConnectionVM> ReadApplicationUserRoomConnectionAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRoomsConnections
                .Include(s => s.ApplicationUserRoom)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomConnectionVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserRoomConnectionVM[]> ReadApplicationUserRoomConnectionByApplicationUserRoomIdAsync(Guid applicationUserRoomId)
        {
            var entities = await _context.ApplicationUsersRoomsConnections
                .Where(s => s.ApplicationUserRoomId == applicationUserRoomId && s.Active)
                .ToListAsync();

            var vms = new List<ApplicationUserRoomConnectionVM>();
            entities.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(s)));

            return vms.ToArray();
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
            var entity = await _context.ApplicationUsersRoomsConnections
                .FirstOrDefaultAsync(s => s.ConnectionId == request.ConnectionId);

            if (entity == null)
            {
                entity = _mapper.Map<ApplicationUserRoomConnection>(request);
                await _context.ApplicationUsersRoomsConnections.AddAsync(entity);
            }
            else
            {
                entity.IsOnline = request.IsOnline;
                entity.HasPingBeenSent = request.HasPingBeenSent;
                entity.ConnectionType = request.ConnectionType;
                _context.Entry(entity).State = EntityState.Modified;
            }

            return await _context.SaveChangesAsync() > 0 ? await ReadApplicationUserRoomConnectionAsync(entity.ConnectionId) : null;
        }
        public virtual async Task<ApplicationUserRoomConnectionVM> UpdateApplicationUserRoomConnectionAsync(ApplicationUserRoomConnectionUpdateRequest request)
        {
            var entity = await _context.ApplicationUsersRoomsConnections
                    .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active);

            if (entity != null)
            {
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
        //public virtual async Task<CurrencyDTO[]> ReadCurrenciesAsync()
        //{
        //    var entities = await _context.Currencies
        //        .Where(s => s.Active)
        //        .ToListAsync();

        //    var dtos = new List<CurrencyDTO>();

        //    entities.ForEach(s => dtos.Add(_mapper.Map<CurrencyDTO>(s)));

        //    return dtos.ToArray();
        //}
        //public virtual async Task<CurrencyVM> CreateCurrencyAsync(CurrencyCreateRequest request, Guid applicationUserId)
        //{
        //    var entity = _mapper.Map<Currency>(request);

        //    _context.Currencies.Add(entity);

        //    return await _context.SaveChangesAsync() > 0 ? await ReadCurrencyAsync(entity.Id) : null;
        //}
        //public virtual async Task<CurrencyVM> UpdateCurrencyAsync(CurrencyCreateRequest request, Guid applicationUserId)
        //{
        //    var entity = await _context.Currencies
        //        .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active);

        //    if (entity != null)
        //    {
        //        entity.CurrencyName = request.CurrencyName;
        //        entity.Weight = request.Weight;
        //        entity.QuantityIncreasePerTick = request.QuantityIncreasePerTick;
        //        entity.TimeSecBetweenTick = request.TimeSecBetweenTick;
        //        _context.Entry(entity).State = EntityState.Modified;

        //        if (await _context.SaveChangesAsync() > 0)
        //        {
        //            return await ReadCurrencyAsync(entity.Id);
        //        }
        //    }
        //    return null;
        //}
        //public virtual async Task<bool> DeleteCurrencyAsync(Guid id)
        //{
        //    var entity = await _context.Currencies
        //        .FirstOrDefaultAsync(s => s.Id == id && s.Active);

        //    if (entity != null)
        //    {
        //        entity.Active = false;
        //        _context.Entry(entity).State = EntityState.Modified;
        //        if (await _context.SaveChangesAsync() > 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        
        // return all playlists for current user
        public virtual async Task<PlaylistDTO[]> ReadPlaylistsAsync(Guid applicationUserId)
        {
            var entities = await _context.Playlists
                .Include(s => s.SongsPlaylist)
                .Where(s => s.ApplicationUserId == applicationUserId && s.Active)
                .ToListAsync();

            var dtos = new List<PlaylistDTO>();

            entities.ForEach(s => dtos.Add(_mapper.Map<PlaylistDTO>(s)));

            return dtos.ToArray();
        }
        public virtual async Task<PlaylistVM> ReadPlaylistAsync(Guid id, Guid applicationUserId)
        {
            var entity = await _context.Playlists
                .Include(s => s.SongsPlaylist)
                .FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == applicationUserId && s.Active);

            return entity != null ? _mapper.Map<PlaylistVM>(entity) : null;
        }
        public virtual async Task<PlaylistVM> CreatePlaylistAsync(PlaylistCreateRequest request, Guid applicationUserId)
        {
            var user = await _context.ApplicationUsers
                .Include(s => s.Playlists)
                .FirstOrDefaultAsync(s => s.Id == applicationUserId);

            if (user.Playlists.Where(s => s.Active).ToList().Count < user.PlaylistCountMax)
            {
                var entity = _mapper.Map<Playlist>(request);

                // this validation so only 1 playlist is listed as default
                var otherPlaylists = await _context.Playlists
                    .Where(s => s.ApplicationUserId == applicationUserId)
                    .ToListAsync();
                // if this playlist is default
                if (entity.IsSelected)
                {
                    foreach (var otherPlaylist in otherPlaylists)
                    {
                        otherPlaylist.IsSelected = false;
                        _context.Entry(otherPlaylist).State = EntityState.Modified;
                    }
                }
                else if (otherPlaylists.Count <= 0 || !otherPlaylists.Any(s => s.IsSelected))
                {
                    entity.IsSelected = true;
                }

                entity.ApplicationUserId = applicationUserId;
                await _context.Playlists.AddAsync(entity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadPlaylistAsync(entity.Id, applicationUserId);
                }
            }

            return null;
        }
        public virtual async Task<PlaylistVM> UpdatePlaylistAsync(PlaylistCreateRequest request, Guid applicationUserId)
        {
            var entity = await _context.Playlists
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.ApplicationUserId == applicationUserId && s.Active);

            if (entity != null)
            {
                entity.PlaylistName = request.PlaylistName;
                entity.IsSelected = request.IsSelected;
                _context.Entry(entity).State = EntityState.Modified;

                // this validation so only 1 playlist is listed as default
                var otherPlaylists = await _context.Playlists
                    .Where(s => s.ApplicationUserId == applicationUserId && s.Id != entity.Id)
                    .ToListAsync();
                // if this playlist is default
                if (entity.IsSelected)
                {
                    foreach (var otherPlaylist in otherPlaylists)
                    {
                        otherPlaylist.IsSelected = false;
                        _context.Entry(otherPlaylist).State = EntityState.Modified;
                    }
                }
                else if (otherPlaylists.Count() <= 0 || !otherPlaylists.Any(s => s.IsSelected))
                {
                    entity.IsSelected = true;
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadPlaylistAsync(entity.Id, applicationUserId);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeletePlaylistAsync(Guid id, Guid applicationUserId)
        {
            var entity = await _context.Playlists
                .FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == applicationUserId && s.Active);

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
                .Include(s => s.SongRequests)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<SongVM>(entity) : null;
        }
        public virtual async Task<SongVM> ReadSongAsync(string videoId)
        {
            var entity = await _context.Songs
                .FirstOrDefaultAsync(s => s.YoutubeId == videoId && s.Active);

            return entity != null ? _mapper.Map<SongVM>(entity) : null;
        }
        public virtual async Task<SongVM> CreateSongAsync(SongCreateRequest request)
        {

            var entity = _mapper.Map<Song>(request);

            await _context.Songs.AddAsync(entity);

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

        //public virtual async Task<SongQueuedVM[]> QueuePlaylistInRoomHomeAsync(Guid playlistId, Guid applicationUserId)
        //{
        //    var applicationUser = await _context.ApplicationUsers
        //        .FirstOrDefaultAsync(s => s.Id == applicationUserId && s.Active);

        //    var room = await _context.Rooms
        //        .FirstOrDefaultAsync(s => s.ApplicationUserId == applicationUserId && s.Active);

        //    var playlist = await _context.Playlists
        //        .FirstOrDefaultAsync(s => s.Id == playlistId && s.Active);

        //    if (room != null && playlist != null)
        //    {
        //        var songsQueued = await _context.SongsQueued
        //            .Where(s => s.RoomId == room.Id && !s.HasBeenPlayed && s.Active)
        //            .ToListAsync();

        //        var songsPlaylsit = await _context.SongsPlaylists
        //            .Where(s => s.PlaylistId == playlist.Id && s.Active)
        //            .ToListAsync();

        //        var counter = songsQueued.Count();

        //        foreach (var songPlaylist in songsPlaylsit)
        //        {
        //            if (counter < applicationUser.QueueCount && !songsQueued.Any(s=> s.SongId == songPlaylist.SongId))
        //            {
        //                counter++;

        //                _context.SongsQueued.Add(new SongQueued
        //                {
        //                    ApplicationUserId = applicationUserId,
        //                    RoomId = room.Id,
        //                    SongId = songPlaylist.SongId,
        //                    WeightedValue = 0,
        //                    TransactionsSongQueued = new List<TransactionSongQueued>
        //                    {
        //                        new TransactionSongQueued
        //                        {
        //                            TransactionType = TransactionType.Request
        //                        }
        //                    }
        //                });
        //            }
        //        }
        //    }
        //}
        public virtual async Task<SongQueuedVM> QueueSongPlaylistNext(Guid applicationUserId)
        {
            var playlist = await _context.Playlists
                .Where(s => s.ApplicationUserId == applicationUserId && s.Active && s.IsSelected)
                .FirstOrDefaultAsync();

            if (playlist != null)
            {
                var songPlaylist = await _context.SongsPlaylists
                    .Include(s => s.Song)
                    .Include(s => s.Playlist)
                    .Where(s => s.PlaylistId == playlist.Id && s.Active)
                    .OrderBy(s => s.PlayCount)
                    .FirstOrDefaultAsync();

                if (songPlaylist != null)
                {
                    songPlaylist.PlayCount++;
                    _context.Entry((SongPlaylist)songPlaylist).State = EntityState.Modified;

                    var applicationUser = await _context.ApplicationUsers
                        .Include(s => s.Room)
                        .FirstOrDefaultAsync(s => s.Id == applicationUserId);

                    var queuedSong = new SongQueued
                    {
                        ApplicationUser = applicationUser,
                        Room = applicationUser.Room,
                        Song = songPlaylist.Song,
                        SongRequestType = SongRequestType.Playlist,
                        WeightedValue = 0,
                    };

                    await _context.SongsQueued.AddAsync(queuedSong);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<SongQueuedVM>(queuedSong);
                    }
                }
            }
            return null;
        }
        public virtual async Task<SongPlaylistVM[]> ReadSongsPlaylistAsync(Guid playlistId)
        {
            var entities = await _context.SongsPlaylists
                .Include(s => s.Song)
                .Where(s => s.Playlist.Id == playlistId && s.Active)
                .ToListAsync();

            var dtos = new List<SongPlaylistVM>();

            entities.ForEach(s => dtos.Add(_mapper.Map<SongPlaylistVM>(s)));

            return dtos.ToArray();
        }
        public virtual async Task RestartSongPlaylistCountAsync(Guid applicationUserId)
        {
            var playlists = await _context.Playlists
                .Where(s => s.ApplicationUserId == applicationUserId)
                .ToListAsync();

            foreach (var playlist in playlists)
            {
                var songsPlaylist = await _context.SongsPlaylists
                    .Where(s => s.PlaylistId == playlist.Id)
                    .ToListAsync();

                foreach (var songPlaylist in songsPlaylist)
                {
                    songPlaylist.PlayCount = 0;
                    _context.Entry(songPlaylist).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();
        }
        public virtual async Task<SongPlaylistVM> ReadSongPlaylistAsync(Guid id)
        {
            var entity = await _context.SongsPlaylists
                .Include(s => s.Song)
                .Include(s => s.Playlist)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<SongPlaylistVM>(entity) : null;
        }
        public virtual async Task<SongPlaylistVM> CreateSongPlaylistAsync(SongPlaylistCreateRequest request, Guid applicationUserId)
        {

            var song = await _context.Songs
                .FirstOrDefaultAsync(s => s.YoutubeId == request.SongSearchResult.VideoId);

            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(s => s.Id == request.PlaylistId && s.ApplicationUserId == applicationUserId && s.Active);

            if (playlist != null)
            {
                if (song == null)
                {
                    // Create the Song
                    YoutubeSearchResponse response;
                    using (var httpClient = new HttpClient())
                    {
                        //var url = $"https://www.googleapis.com/youtube/v3/videos?id={request.SongSearchResult.VideoId}&part=snippet";
                        var url = $"https://www.googleapis.com/youtube/v3/videos?id={request.SongSearchResult.VideoId}&part=contentDetails&key={Globals.GOOGLE_API_KEY}";
                        var result = await httpClient.GetStringAsync(url);
                        response = JsonConvert.DeserializeObject<YoutubeSearchResponse>(result);
                    }

                    var parsedLength = response.Items[0].ContentDetails.Duration.Substring(2);
                    var hours = 0f;
                    var minutes = 0f;
                    var seconds = 0f;

                    if (parsedLength.IndexOf("H") > 0)
                    {
                        if (float.TryParse(parsedLength.Substring(0, parsedLength.IndexOf("H")), out var hoursInt))
                        {
                            hours = hoursInt;
                            parsedLength = parsedLength.Substring(parsedLength.IndexOf("H") + 1);
                        }
                    }

                    if (parsedLength.IndexOf("M") > 0)
                    {
                        if (float.TryParse(parsedLength.Substring(0, parsedLength.IndexOf("M")), out var minutesInt))
                        {
                            minutes = minutesInt;
                            parsedLength = parsedLength.Substring(parsedLength.IndexOf("M") + 1);
                        }
                    }

                    if (parsedLength.IndexOf("S") > 0)
                    {
                        if (float.TryParse(parsedLength.Substring(0, parsedLength.IndexOf("S")), out var secondsInt))
                        {
                            seconds = secondsInt;
                            parsedLength = parsedLength.Substring(parsedLength.IndexOf("S") + 1);
                        }
                    }

                    var time = (int)Math.Round(TimeSpan.FromHours(hours).Add(TimeSpan.FromMinutes(minutes)).Add(TimeSpan.FromSeconds(seconds)).TotalSeconds);
                    var songVM = await CreateSongAsync(new SongCreateRequest
                    {
                        YoutubeId = request.SongSearchResult.VideoId,
                        SongLengthSeconds = time,
                        SongName = request.SongSearchResult.SongName
                    });

                    song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == songVM.Id);
                }

                // validation for checking is exist, not to add the same song to playlist
                var songPlaylist = await _context.SongsPlaylists
                    .FirstOrDefaultAsync(s => s.PlaylistId == request.PlaylistId &&
                        s.SongId == song.Id && s.Active);

                if (songPlaylist  == null)
                {
                    songPlaylist = new SongPlaylist
                    {
                        Playlist = playlist,
                        Song = song
                    };

                    await _context.SongsPlaylists.AddAsync(songPlaylist);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<SongPlaylistVM>(songPlaylist);
                    }
                }
            }
            return null;

            //var entity = _mapper.Map<SongPlaylist>(request);

            //_context.SongsPlaylists.Add(entity);

            //if (await _context.SaveChangesAsync() > 0)
            //{
            //    return await ReadSongPlaylistAsync(entity.Id);
            //}

            //return null;
        }
        public virtual async Task<bool> DeleteSongPlaylistAsync(Guid id, Guid applicationUserId)
        {
            var entity = await _context.SongsPlaylists
                .FirstOrDefaultAsync(s => s.Id == id && s.Playlist.ApplicationUserId == applicationUserId && s.Active);

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

        public virtual async Task<SongQueuedVM[]> ReadSongsQueuedAsync(Guid roomId)
        {
            var entities = await _context.SongsQueued
                .Include(s => s.Song)
                .Include(s => s.Room)
                .Include(s => s.ApplicationUser)
                .Where(s => s.RoomId == roomId && s.Active)
                .ToListAsync();

            var vms = new List<SongQueuedVM>();
            entities.ForEach(s => vms.Add(_mapper.Map<SongQueuedVM>(s)));

            return vms.ToArray();
        }
        public virtual async Task<SongQueuedVM> ReadSongQueuedAsync(Guid id)
        {
            var entity = await _context.SongsQueued
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<SongQueuedVM>(entity) : null;
        }
        public virtual async Task<SongQueuedVM> CreateSongQueuedAsync(SongQueuedCreateRequest request)
        {

            // Validation
            if (request.SongSearchResult.QuantityWagered <= 0)
            {
                return null;
            }

            var applicationUserRoomCurrency = await ReadApplicationUserRoomCurrencyRoomAsync(request.SongSearchResult.ApplicationUserRoomCurrencyId);

            if (applicationUserRoomCurrency != null &&
                applicationUserRoomCurrency.Quantity >= request.SongSearchResult.QuantityWagered)
            {
                var applicationUserRoom = await ReadApplicationUserRoomAsync(applicationUserRoomCurrency.ApplicationUserRoom.Id);

                if (applicationUserRoom != null)
                {
                    var song = await ReadSongAsync(request.SongSearchResult.VideoId);

                    if (song == null)
                    {
                        // Create the Song
                        YoutubeSearchResponse response;
                        using (var httpClient = new HttpClient())
                        {
                            //var url = $"https://www.googleapis.com/youtube/v3/videos?id={request.SongSearchResult.VideoId}&part=snippet";
                            var url = $"https://www.googleapis.com/youtube/v3/videos?id={request.SongSearchResult.VideoId}&part=contentDetails&key={Globals.GOOGLE_API_KEY}";
                            var result = await httpClient.GetStringAsync(url);
                            response = JsonConvert.DeserializeObject<YoutubeSearchResponse>(result);
                        }

                        var parsedLength = response.Items[0].ContentDetails.Duration.Substring(2);
                        var hours = 0f;
                        var minutes = 0f;
                        var seconds = 0f;

                        if (parsedLength.IndexOf("H") > 0)
                        {
                            if (float.TryParse(parsedLength.Substring(0, parsedLength.IndexOf("H")), out var hoursInt))
                            {
                                hours = hoursInt;
                                parsedLength = parsedLength.Substring(parsedLength.IndexOf("H") + 1);
                            }
                        }

                        if (parsedLength.IndexOf("M") > 0)
                        {
                            if (float.TryParse(parsedLength.Substring(0, parsedLength.IndexOf("M")), out var minutesInt))
                            {
                                minutes = minutesInt;
                                parsedLength = parsedLength.Substring(parsedLength.IndexOf("M") + 1);
                            }
                        }

                        if (parsedLength.IndexOf("S") > 0)
                        {
                            if (float.TryParse(parsedLength.Substring(0, parsedLength.IndexOf("S")), out var secondsInt))
                            {
                                seconds = secondsInt;
                                parsedLength = parsedLength.Substring(parsedLength.IndexOf("S") + 1);
                            }
                        }

                        var time = (int)Math.Round(TimeSpan.FromHours(hours).Add(TimeSpan.FromMinutes(minutes)).Add(TimeSpan.FromSeconds(seconds)).TotalSeconds);
                        song = await CreateSongAsync(new SongCreateRequest
                        {
                            YoutubeId = request.SongSearchResult.VideoId,
                            SongLengthSeconds = time,
                            SongName = request.SongSearchResult.SongName
                        });

                    }

                    var songsQueued = await ReadSongsQueuedAsync(applicationUserRoom.Room.Id);

                    var songQueued = await _context.SongsQueued
                        .FirstOrDefaultAsync(s => s.SongId == song.Id && s.Active);

                    // validation not to add the same song to the queue
                    if (songQueued == null)
                    {
                        songQueued = new SongQueued
                        {
                            ApplicationUserId = applicationUserRoom.ApplicationUser.Id,
                            RoomId = applicationUserRoom.Room.Id,
                            SongId = song.Id,
                            WeightedValue = request.SongSearchResult.QuantityWagered * applicationUserRoomCurrency.CurrencyRoom.Currency.Weight,
                            TransactionsSongQueued = new List<TransactionSongQueued>
                            {
                                new TransactionSongQueued
                                {
                                    ApplicationUserRoomCurrencyId = applicationUserRoomCurrency.Id,
                                    QuantityChange = request.SongSearchResult.QuantityWagered,
                                    TransactionType = TransactionType.Request
                                }
                            }
                        };
                        await _context.SongsQueued.AddAsync(songQueued);
                    }
                    else
                    {
                        songQueued.WeightedValue += request.SongSearchResult.QuantityWagered * applicationUserRoomCurrency.CurrencyRoom.Currency.Weight;
                        _context.Entry(songQueued).State = EntityState.Modified;
                    }
                    
                    var applicationUserRoomCurrencyEntity = await _context.ApplicationUsersRoomsCurrenciesRooms
                        .FirstOrDefaultAsync(s => s.Id == applicationUserRoomCurrency.Id);

                    if (applicationUserRoomCurrencyEntity != null)
                    {
                        applicationUserRoomCurrencyEntity.Quantity -= request.SongSearchResult.QuantityWagered;
                        _context.Entry(applicationUserRoomCurrencyEntity).State = EntityState.Modified;

                        if (applicationUserRoomCurrencyEntity.Quantity >= 0 && await _context.SaveChangesAsync() > 0)
                        {
                            await _context.SaveChangesAsync();
                            return await ReadSongQueuedAsync(songQueued.Id);
                        }
                    }
                }
            }

            //var entity = _mapper.Map<SongQueued>(request);

            //_context.SongsQueued.Add(entity);

            //if (await _context.SaveChangesAsync() > 0)
            //{
            //    return await ReadSongQueuedAsync(entity.Id);
            //}

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
        public virtual async Task<SongQueuedVM> DequeueSongQueuedAsync(Guid roomId, Guid applicationUserId)
        {
            var queuedSongNext = await _context.SongsQueued
                .Include(s => s.Room)
                .Where(s => s.RoomId == roomId && s.ApplicationUserId == applicationUserId && s.Active)
                .OrderByDescending(s => s.WeightedValue)
                .Include(s => s.ApplicationUser)
                .Include(s => s.Song)
                .FirstOrDefaultAsync();

            if (queuedSongNext == null)
            {
                // Pull a song from the playlist
                var nextSongPlaylist = await QueueSongPlaylistNext(applicationUserId);

                if (nextSongPlaylist != null)
                {
                    return nextSongPlaylist;
                }
            }
            else
            {
                queuedSongNext.Active = false;
                queuedSongNext.HasBeenPlayed = true;
                queuedSongNext.TimestampPlayed = DateTime.UtcNow;
                _context.Entry(queuedSongNext).State = EntityState.Modified;

                //_context.SongsQueued.Remove(queuedSongNext);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return _mapper.Map<SongQueuedVM>(queuedSongNext);
                }
            }
            return null;
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

            await _context.Transactions.AddAsync(entity);

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

        public virtual async Task<bool> WagerQuantitySongQueued(WagerQuantitySongQueuedRquest request)
        {
            var applicationUserRoomCurrency = await _context.ApplicationUsersRoomsCurrenciesRooms
                .Include(s => s.CurrencyRoom)
                .Include(s => s.CurrencyRoom.Currency)
                .FirstOrDefaultAsync(s => s.Id == request.ApplicationUserRoomCurrencyRoom.Id && s.Active);

            if (applicationUserRoomCurrency != null && applicationUserRoomCurrency.Quantity >= Math.Abs(request.Quantity))
            {
                var songQueued = await _context.SongsQueued
                    .FirstOrDefaultAsync(s => s.Id == request.SongQueued.Id);

                if (songQueued != null)
                {
                    applicationUserRoomCurrency.Quantity -= Math.Abs(request.Quantity);
                    _context.Entry(applicationUserRoomCurrency).State = EntityState.Modified;

                    songQueued.WeightedValue += request.Quantity * applicationUserRoomCurrency.CurrencyRoom.Currency.Weight;

                    songQueued.TransactionsSongQueued.Add(new TransactionSongQueued
                    {
                        ApplicationUserRoomCurrencyId = applicationUserRoomCurrency.Id,
                        QuantityChange = request.Quantity,
                        TransactionType = TransactionType.Wager
                    });

                    if (songQueued.WeightedValue <= 0)
                    {
                        // the song should be removed from the queue
                        songQueued.Active = false;
                    }

                    _context.Entry(songQueued).State = EntityState.Modified;

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return true;
                    }
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

            await _context.TransactionsSongsQueued.AddAsync(entity);

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

        public virtual async Task<CurrencyRoomVM[]> ReadCurrenciesRoomAsync(Guid roomId)
        {

            await CheckCurrenciesRoomAsync(roomId);

            var entities = await _context.CurrenciesRooms
                .Include(s => s.Currency)
                .Include(s => s.ApplicationUsersRoomsCurrenciesRooms)
                .Include(s => s.Room)
                .Where(s => s.RoomId == roomId && s.Active)
                .ToListAsync();

            var vms = new List<CurrencyRoomVM>();

            entities.ForEach(s => vms.Add(_mapper.Map<CurrencyRoomVM>(s)));

            return vms.ToArray();
            
        }
        public virtual async Task<CurrencyRoomVM[]> CheckCurrenciesRoomAsync(Guid roomId)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(s => s.Id == roomId && s.Active);

            if (room != null)
            {
                var currencies = await _context.Currencies
                    .Where(s => s.Active)
                    .ToListAsync();

                foreach (var currency in currencies)
                {
                    var currencyRoom = await _context.CurrenciesRooms
                        .FirstOrDefaultAsync(s => s.CurrencyId == currency.Id &&
                            s.RoomId == roomId &&
                            s.Active);

                    if (currencyRoom == null)
                    {
                        await _context.CurrenciesRooms.AddAsync(new CurrencyRoom
                        {
                            CurrencyId = currency.Id,
                            CurrencyName = currency.CurrencyName,
                            RoomId = roomId
                        });
                    }
                }

                return await _context.SaveChangesAsync() > 0 ? await ReadCurrenciesRoomAsync(roomId) : null;
            }
            return null;
        }
        public virtual async Task<CurrencyRoomVM> ReadCurrencyRoomAsync(Guid id)
        {
            var entity = await _context.CurrenciesRooms
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<CurrencyRoomVM>(entity) : null;
        }
        public virtual async Task<CurrencyRoomVM> CreateCurrencyRoomAsync(CurrencyRoomCreateRequest request, Guid applicationUserId)
        {
            var entity = _mapper.Map<CurrencyRoom>(request);

            await _context.CurrenciesRooms.AddAsync(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadCurrencyRoomAsync(entity.Id) : null;
        }
        public virtual async Task<CurrencyRoomVM> UpdateCurrencyRoomAsync(CurrencyRoomCreateRequest request, Guid applicationUserId)
        {
            var entity = await _context.CurrenciesRooms
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active);

            if (entity != null)
            {
                entity.CurrencyName = request.CurrencyName;
                entity.CurrencyId = request.CurrencyId;
                entity.RoomId = request.RoomId;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadCurrencyRoomAsync(entity.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteCurrencyRoomAsync(Guid id)
        {
            var entity = await _context.CurrenciesRooms
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
            await _context.LogsAPI.AddAsync(entity);

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
            await _context.LogsErrors.AddAsync(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadLogErrorAsync(entity.Id) : null;
        }

        public virtual async Task<RoomDTO[]> ReadRoomsAsync()
        {
            var entities = await _context.Rooms
                    .Where(s => s.IsRoomOnline && s.IsRoomPublic && s.Active)
                    .ToListAsync();

            var dtos = new List<RoomDTO>();

            foreach (var entity in entities)
            {
                var usersOnline = await _context.ApplicationUsersRooms
                    .Where(s => s.RoomId == entity.Id && s.IsOnline && s.Active)
                    .CountAsync();

                var dto = _mapper.Map<RoomDTO>(entity);
                dto.NumberUsersOnline = usersOnline;
                dtos.Add(dto);
            }

            //entities.ForEach(s => dtos.Add(_mapper.Map<RoomDTO>(s)));

            return dtos.ToArray();
        }
        public virtual async Task<RoomVM> ReadRoomAsync(Guid id)
        {
            var entity = await _context.Rooms
                .Include(s => s.CurrenciesRoom)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                var currencies = await _context.CurrenciesRooms
                    .Where(s => s.RoomId == entity.Id && s.Active)
                    .ToListAsync();

                var vm = _mapper.Map<RoomVM>(entity);

                var usersOnline = await _context.ApplicationUsersRooms
                    .Where(s => s.RoomId == entity.Id && s.IsOnline && s.Active)
                    .CountAsync();

                vm.NumberUsersOnline = usersOnline;
                return vm;
            }

            return null;
        }
        public virtual async Task<RoomVM> ReadRoomAsync(string roomCode)
        {
            var entity = await _context.Rooms
                .FirstOrDefaultAsync(s => s.RoomCode.Trim().ToLower() == roomCode.Trim().ToLower() && s.Active);
            
            if (entity != null)
            {
                var currencies = await _context.CurrenciesRooms
                    .Where(s => s.RoomId == entity.Id && s.Active)
                    .ToListAsync();

                var vm = _mapper.Map<RoomVM>(entity);

                var usersOnline = await _context.ApplicationUsersRooms
                    .Where(s => s.RoomId == entity.Id && s.IsOnline && s.Active)
                    .CountAsync();

                vm.NumberUsersOnline = usersOnline;
                return vm;
            }

            return null;
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

        public virtual async Task<ICollection<PurchasableItemDTO>> ReadPurchasableItemsAsync()
        {
            var purchasablesItems = await _context.PurchasableItems
                .Where(s => s.Active)
                .ToListAsync();

            var dtos = new List<PurchasableItemDTO>();

            purchasablesItems.ForEach(s => dtos.Add(_mapper.Map<PurchasableItemDTO>(s)));

            return dtos;
        }
        public virtual async Task<PurchasableItemVM> ReadPurchasableItemAsync(Guid id)
        {
            var entity = await _context.PurchasableItems
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<PurchasableItemVM>(entity) : null;
        }
        public virtual async Task<PurchasableItemVM> CreatePurchasableItemAsync(PurchasableItemCreateRequest request)
        {
            var entity = _mapper.Map<PurchasableItem>(request);

            //for (int i = 0; i < Enum.GetNames(typeof(PurchasableItemType)).Length; i++)
            //{
            //    if (request.PurchasableItemType == ((PurchasableItemType)i).ToString())
            //    {
            //        entity.PurchasableItemType = (PurchasableItemType)i;
            //        break;
            //    }
            //}

            await _context.PurchasableItems.AddAsync(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadPurchasableItemAsync(entity.Id) : null;
        }
        public virtual async Task<PurchasableItemVM> UpdatePurchasableItemAsync(PurchasableItemCreateRequest request)
        {
            var entity = await _context.PurchasableItems
                .FirstOrDefaultAsync(s => s.Id == request.Id);

            _mapper.Map(request, entity);

            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0 ? await ReadPurchasableItemAsync(entity.Id) : null;
        }
        public virtual async Task<bool> DeletePurchasableItemAsync(Guid id, Guid applicationUserId)
        {
            var entity = await _context.PurchasableItems
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

        public virtual async Task<ICollection<PurchaseDTO>> ReadPurchasesAsync()
        {
            var purchases = await _context.Purchases
                .Where(s => s.Active)
                .OrderByDescending(s => s.TimeStamp)
                .ThenByDescending(s => s.AmountCharged)
                .ToListAsync();

            var dtos = new List<PurchaseDTO>();

            purchases.ForEach(s => dtos.Add(_mapper.Map<PurchaseDTO>(s)));

            return dtos;
        }
        public virtual async Task<ICollection<PurchaseDTO>> ReadPurchasesAsync(Guid applicationUserId)
        {
            var purchases = await _context.Purchases
                .Where(s => s.ApplicationUserId == applicationUserId && s.Active)
                .OrderByDescending(s => s.TimeStamp)
                .ThenByDescending(s => s.AmountCharged)
                .ToListAsync();

            var dtos = new List<PurchaseDTO>();

            purchases.ForEach(s => dtos.Add(_mapper.Map<PurchaseDTO>(s)));

            return dtos;
        }
        public virtual async Task<PurchaseVM> ReadPurchaseAsync(Guid id, Guid applicationUserId)
        {
            var entity = await _context.Purchases
                .FirstOrDefaultAsync(s => s.Id == id &&
                    s.ApplicationUserId == applicationUserId
                    && s.Active);

            var purchaseLineItems = await _context.PurchaseLineItems
                .Where(s => s.PurchaseId == entity.Id && s.Active)
                .ToListAsync();

            return entity != null ? _mapper.Map<PurchaseVM>(entity) : null;
        }
        public virtual async Task<PurchaseVM> CreatePurchaseAsync(PurchaseCreateRequest request, Guid applicationUserId)
        {
            var applicationUser = await _context.ApplicationUsers
                .FirstOrDefaultAsync(s => s.Id == applicationUserId && s.Active);

            if (applicationUser != null)
            {
                var entity = new Purchase
                {
                    Active = true,
                    AmountCharged = request.AmountCharged,
                    ApplicationUserId = applicationUserId,
                    PurchaseMethod = request.PurchaseMethod,
                    Subtotal = request.Subtotal,
                    HasBeenCharged = true,
                    PayerId = request.PayerId,
                    OrderId = request.OrderId,
                    WasChargeAccepted = true
                };

                await _context.Purchases.AddAsync(entity);
                
                foreach (var item in request.PurchasableItemsJSON)
                {
                    var deserializedPurchasableItem = JsonConvert.DeserializeObject<PurchasableLineItemCreateRequest>(item);

                    var purchasableItem = await _context.PurchasableItems
                        .FirstOrDefaultAsync(s => s.Id == deserializedPurchasableItem.PurchasableItem.Id && s.Active);

                    if (purchasableItem != null)
                    {
                        switch (purchasableItem.PurchasableItemType)
                        {
                            case PurchasableItemType.Playlist:
                                applicationUser.PlaylistCountMax += purchasableItem.Quantity * deserializedPurchasableItem.OrderQuantity;
                                _context.Entry(applicationUser).State = EntityState.Modified;
                                await _context.PurchaseLineItems.AddAsync(new PurchaseLineItem
                                {
                                    PurchasableItem = purchasableItem,
                                    Purchase = entity,
                                    OrderQuantity = deserializedPurchasableItem.OrderQuantity
                                });

                                break;
                            case PurchasableItemType.PlyalistSongs:
                                applicationUser.PlaylistSongCount += purchasableItem.Quantity * deserializedPurchasableItem.OrderQuantity;
                                _context.Entry(applicationUser).State = EntityState.Modified;
                                await _context.PurchaseLineItems.AddAsync(new PurchaseLineItem
                                {
                                    PurchasableItem = purchasableItem,
                                    Purchase = entity,
                                    OrderQuantity = deserializedPurchasableItem.OrderQuantity
                                });

                                break;
                            case PurchasableItemType.PurchaseCurrency:
                                var deserializedPurchasableCurrency = JsonConvert.DeserializeObject<PurchasableLineItemCurrencyCreateRequest>(item);

                                var applicationUserRoomCurrency = await _context.ApplicationUsersRoomsCurrenciesRooms
                                    .FirstOrDefaultAsync(s => s.Id == deserializedPurchasableCurrency.ApplicationUserRoomCurrencyId && s.Active);

                                if (applicationUserRoomCurrency != null)
                                {
                                    applicationUserRoomCurrency.Quantity += deserializedPurchasableItem.PurchasableItem.Quantity * deserializedPurchasableItem.OrderQuantity;
                                    _context.Entry(applicationUserRoomCurrency).State = EntityState.Modified;

                                    await _context.PurchaseLineItems.AddAsync(new PurchaseLineItemCurrency
                                    {
                                        PurchasableItem = purchasableItem,
                                        Purchase = entity,
                                        ApplicationUserRoomCurrencyId = applicationUserRoomCurrency.Id,
                                        OrderQuantity = deserializedPurchasableItem.OrderQuantity
                                    });

                                }
                                break;
                        }
                    }
                    else
                    {
                        // Error if not sending the correct item
                        return null;
                    }
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadPurchaseAsync(entity.Id, applicationUserId);
                }
            }
            return null;
        }

        public virtual async Task<YoutubeResults> SearchYoutubeLightAsync(string searchSnippet)
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = Globals.GOOGLE_API_KEY,
                    ApplicationName = "Listify"
                });

                var searchListRequest = youtubeService.Search.List("snippet");

                searchListRequest.Q = searchSnippet; // replace with your search term
                searchListRequest.MaxResults = 50;

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = await searchListRequest.ExecuteAsync();

                var results = new YoutubeResults();
                var sb = new StringBuilder();

                // Add each result to the appropriate list, and then display the lists of
                // matching videos, channels, and playlists.
                foreach (var searchResult in searchListResponse.Items)
                {
                    switch (searchResult.Id.Kind)
                    {
                        case "youtube#video":
                            results.Results.Add(new YoutubeResults.YoutubeResult
                            {
                                VideoId = searchResult.Id.VideoId,
                                LengthSec = 0,
                                SongName = searchResult.Snippet.Title
                            });
                            break;
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public virtual async Task<YoutubeResults> SearchYoutubeAsync(string searchSnippet)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Globals.GOOGLE_API_KEY,
                ApplicationName = "Listify" 
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = searchSnippet; // replace with your search term
            searchListRequest.MaxResults = 100;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            var youtubeResults = new YoutubeResults();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                            youtubeResults.Results.Add(new YoutubeResults.YoutubeResult
                            {
                                VideoId = searchResult.Id.VideoId,
                                SongName = searchResult.Snippet.Title,
                                LengthSec = 0
                            });
                        break;
                }
            }
            return youtubeResults;
        }

        public virtual async Task<bool> CheckAuthToLockedRoomAsync(string roomKey, Guid roomId)
        {
            var encodedRoomKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(roomKey));

            return await _context.Rooms.AnyAsync(s => s.Id == roomId && s.RoomKey == encodedRoomKey);
        }
        public virtual async Task<bool> IsUsernameAvailableAsync(string applicationUsername, Guid applicationUserId)
        {
            return !(await _context.ApplicationUsers.AnyAsync(s => s.Username.Trim().ToLower() == applicationUsername.Trim().ToLower() &&
                s.Id != applicationUserId &&
                s.Active));
        }
        public virtual async Task<bool> IsRoomCodeAvailableAsync(string roomCode, Guid applicationUserId)
        {
            return !(await _context.Rooms.AnyAsync(s => s.RoomCode.Trim().ToLower() == roomCode.Trim().ToLower() &&
                s.ApplicationUserId != applicationUserId &&
                s.Active));
        }

        public async Task<ICollection<ApplicationUserRoomCurrencyRoomVM>> AddCurrencyQuantityToAllUsersInRoomAsync(Guid roomId, Guid currencyRoomId, int currencyQuantity, TransactionType transactionType)
        {
            try
            {
                var applicationUserRoomCurrencies = new List<ApplicationUserRoomCurrencyRoom>();

                // Get Room currency here
                var room = await _context.Rooms
                    .FirstOrDefaultAsync(s => s.Id == roomId);

                var currencyRoom = await _context.CurrenciesRooms
                    .FirstOrDefaultAsync(s => s.Id == currencyRoomId);

                var applicationUserRooms = await _context.ApplicationUsersRooms
                        .Where(s => s.RoomId == room.Id && s.Active && s.IsOnline)
                        .ToListAsync();

                if (room != null && currencyRoom != null)
                {
                    currencyRoom.TimestampLastUpdate = DateTime.UtcNow;
                    _context.Entry(currencyRoom).State = EntityState.Modified;

                    foreach (var applicationUserRoom in room.ApplicationUsersRooms)
                    {
                        if (applicationUserRoom != null)
                        {
                            var applicationUserRoomCurrency = await _context.ApplicationUsersRoomsCurrenciesRooms
                                .FirstOrDefaultAsync(s => s.ApplicationUserRoomId == applicationUserRoom.Id &&
                                    s.Active && s.CurrencyRoomId == currencyRoomId);

                            // ToDo: Delete this line for bad performance to read another value from different db context
                            // that has been already modified
                            await _context.Entry(applicationUserRoomCurrency).ReloadAsync();

                            if (applicationUserRoomCurrency == null)
                            {
                                applicationUserRoomCurrency = new ApplicationUserRoomCurrencyRoom
                                {
                                    ApplicationUserRoomId = applicationUserRoom.Id,
                                    CurrencyRoom = currencyRoom,
                                    Quantity = 0,
                                    TimeStamp = DateTime.UtcNow
                                };

                                await _context.ApplicationUsersRoomsCurrenciesRooms.AddAsync(applicationUserRoomCurrency);
                            }

                            if (!applicationUserRoomCurrencies.Any(s => s.Id == applicationUserRoomCurrency.Id))
                            {
                                applicationUserRoomCurrencies.Add(applicationUserRoomCurrency);
                            }
                        }
                    }

                    foreach (var applicationUserRoomCurrency in applicationUserRoomCurrencies)
                    {
                        var entity = await _context.ApplicationUsersRoomsCurrenciesRooms.FirstOrDefaultAsync(s => s.Id == applicationUserRoomCurrency.Id);

                        if (entity != null)
                        {
                            entity.Quantity += currencyQuantity;
                            _context.Entry(entity).State = EntityState.Modified;

                            var transaction = new Transaction
                            {
                                ApplicationUserRoomCurrencyId = entity.Id,
                                QuantityChange = currencyQuantity,
                                TransactionType = TransactionType.PollingCurrency,
                                TimeStamp = DateTime.UtcNow
                            };

                            await _context.Transactions.AddAsync(transaction);
                        }
                    }

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var vms = new List<ApplicationUserRoomCurrencyRoomVM>();
                        applicationUserRoomCurrencies.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomCurrencyRoomVM>(s)));
                        return vms;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public virtual async Task<ICollection<ApplicationUserRoomConnectionVM>> PingApplicationUsersRoomsConnectionsAsync()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var connectionsPinged = new List<ApplicationUserRoomConnection>();

                    var rooms = await context.Rooms
                        .Where(s => s.Active)
                        .ToListAsync();

                    foreach (var room in rooms)
                    {
                        var applicationUsersRooms = await context.ApplicationUsersRooms
                            .Where(s => s.RoomId == room.Id && s.Active)
                            .ToListAsync();

                        var isRoomOnline = true;

                        foreach (var applicationUserRoom in applicationUsersRooms)
                        {
                            var applicationUserRoomConnections = await context.ApplicationUsersRoomsConnections
                                .Where(s => s.ApplicationUserRoomId == applicationUserRoom.Id && s.Active)
                                .ToListAsync();

                            foreach (var applicationUserRoomConnection in applicationUserRoomConnections)
                            {
                                try
                                {
                                    if (applicationUserRoomConnection.HasPingBeenSent)
                                    {
                                        // Ping was not responded to - remove connection
                                        // total connection per 1 song/ room without ping is 30 minutes
                                        if ((DateTime.UtcNow - applicationUserRoomConnection.TimeStamp).TotalMinutes > 30)
                                        {
                                            context.ApplicationUsersRoomsConnections.Remove(applicationUserRoomConnection);
                                        }
                                        //connectionsRemoved.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(applicationUserRoomConnection));
                                    }
                                    else
                                    {
                                        // Send Ping
                                        applicationUserRoomConnection.HasPingBeenSent = true;
                                        context.Entry(applicationUserRoomConnection).State = EntityState.Modified;
                                        connectionsPinged.Add(applicationUserRoomConnection);
                                    }
                                }
                                catch (Exception ex)
                                { Console.WriteLine(ex.Message); }
                            }

                            applicationUserRoom.IsOnline = await context.ApplicationUsersRoomsConnections
                                .AnyAsync(s => s.ApplicationUserRoomId == applicationUserRoom.Id &&
                                    s.IsOnline && s.Active);
                        }

                        var ownerRooms = await context.ApplicationUsersRooms
                            .Where(s => s.RoomId == room.Id &&
                                s.IsOwner &&
                                s.Active)
                            .ToListAsync();

                        if (ownerRooms.Count > 0)
                        {
                            foreach (var ownerRoom in ownerRooms)
                            {
                                if (!await context.ApplicationUsersRoomsConnections
                                    .Where(s => s.ApplicationUserRoomId == ownerRoom.Id &&
                                        s.IsOnline &&
                                        s.Active)
                                    .AnyAsync())
                                {
                                    isRoomOnline = false;
                                }
                            }
                        }

                        room.IsRoomOnline = isRoomOnline;
                    }

                    //var applicationUsersRoomsConnections = await context.ApplicationUsersRoomsConnections
                    //    .Where(s => s.Active)
                    //    .ToListAsync();

                    //var connectionsRemoved = new List<ApplicationUserRoomConnectionVM>();

                    if (await context.SaveChangesAsync() > 0)
                    {
                        var vms = new List<ApplicationUserRoomConnectionVM>();
                        connectionsPinged.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(s)));
                        return vms;
                        //return new PingPollVM
                        //{
                        //    ApplicationUserRoomConnectionsRemoved = connectionsRemoved,
                        //    ApplicationUserRoomConnectionsToPing = connectionsToPing
                        //};
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        protected virtual async Task<V> CreateAsync<T, U, V>(T request)
            where T : BaseRequest
            where U : BaseEntity
            where V : BaseVM
        {
            // Converting Request To Entity
            var entity = _mapper.Map<U>(request);

            // Add Entity
            await _context.AddAsync(entity);


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
