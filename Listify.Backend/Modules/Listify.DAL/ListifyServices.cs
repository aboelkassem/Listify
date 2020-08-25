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

namespace Listify.DAL
{
    public class ListifyServices : IListifyServices
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public ListifyServices(
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

            var room = new Room
            {
                ApplicationUser = entity,
                RoomCode = request.Username
            };
            _context.Rooms.Add(room);

            var applicationUserRoom = new ApplicationUserRoom
            {
                ApplicationUser = entity,
                IsOnline = true,
                IsOwner = true,
                Room = room
            };


            _context.ApplicationUsersRooms.Add(applicationUserRoom);

            if (await _context.SaveChangesAsync() > 0)
            {
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
                entity.Username = request.Username;
                entity.PlaylistCountMax = request.PlaylistCountMax;
                entity.SongPoolCountSongsMax = request.SongPoolCountSongsMax;
                entity.Room.RoomCode = request.RoomCode;
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
                .Include(s => s.ApplicationUserRoom)
                .Include(s => s.Currency)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomCurrencyVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserRoomCurrencyVM> ReadApplicationUserRoomCurrencyAsync(Guid applicationUserRoomId, Guid currencyId)
        {
            var entity = await _context.ApplicationUsersRoomsCurrencies
                .Include(s => s.ApplicationUserRoom)
                .Include(s => s.Currency)
                .FirstOrDefaultAsync(s => s.ApplicationUserRoomId == applicationUserRoomId &&
                s.CurrencyId == currencyId && s.Active);

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

        public virtual async Task<ApplicationUserRoomConnectionVM[]> ReadApplicationUsersRoomsConnectionsAsync(Guid roomId)
        {
            var connections = await _context.ApplicationUsersRoomsConnections
                .Include(s => s.ApplicationUserRoom)
                .Where(s => s.ApplicationUserRoom.RoomId == roomId && s.Active && s.ApplicationUserRoom.Active)
                .ToListAsync();

            var vms = new List<ApplicationUserRoomConnectionVM>();
            connections.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(s)));

            return vms.ToArray();
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
                _context.ApplicationUsersRoomsConnections.Add(entity);
            }
            else
            {
                entity.IsOnline = request.IsOnline;
                entity.HasPingBeenSent = request.HasPingBeenSent;
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

        public virtual async Task<CurrencyDTO[]> ReadCurrenciesAsync()
        {
            var entities = await _context.Currencies
                .Where(s => s.Active)
                .ToListAsync();

            var dtos = new List<CurrencyDTO>();

            entities.ForEach(s => dtos.Add(_mapper.Map<CurrencyDTO>(s)));

            return dtos.ToArray();
        }

        public virtual async Task<CurrencyVM> ReadCurrencyAsync(Guid id)
        {
            var entity = await _context.Currencies
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<CurrencyVM>(entity) : null;
        }
        public virtual async Task<CurrencyVM> CreateCurrencyAsync(CurrencyCreateRequest request, Guid applicationUserId)
        {
            var entity = _mapper.Map<Currency>(request);

            _context.Currencies.Add(entity);

            return await _context.SaveChangesAsync() > 0 ? await ReadCurrencyAsync(entity.Id) : null;
        }
        public virtual async Task<CurrencyVM> UpdateCurrencyAsync(CurrencyCreateRequest request, Guid applicationUserId)
        {
            var entity = await _context.Currencies
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active);

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
        
        // return all playlists for current user
        public virtual async Task<PlaylistDTO[]> ReadPlaylistsAsync(Guid applicationUserId)
        {
            var entities = await _context.Playlists
                .Include(s => s.SongsPlaylists)
                .Where(s => s.ApplicationUserId == applicationUserId && s.Active)
                .ToListAsync();

            var dtos = new List<PlaylistDTO>();

            entities.ForEach(s => dtos.Add(_mapper.Map<PlaylistDTO>(s)));

            return dtos.ToArray();
        }
        public virtual async Task<PlaylistVM> ReadPlaylistAsync(Guid id, Guid applicationUserId)
        {
            var entity = await _context.Playlists
                .Include(s => s.SongsPlaylists)
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
                else if (otherPlaylists.Count() <= 0 || !otherPlaylists.Any(s => s.IsSelected))
                {
                    entity.IsSelected = true;
                }

                entity.ApplicationUserId = applicationUserId;
                _context.Playlists.Add(entity);

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

        public virtual async Task<SongQueuedVM> QueueSongPlaylistNext(Guid applicationUserId)
        {
            var playlist = await _context.Playlists
                .Where(s => s.ApplicationUserId == applicationUserId && s.Active && s.IsSelected)
                .Include(s => s.SongsPlaylists)
                .FirstOrDefaultAsync();

            if (playlist != null)
            {
                var selectedSong = playlist.SongsPlaylists.OrderBy(s => s.PlayCount).FirstOrDefault();

                if (selectedSong != null)
                {
                    var selectedSongEntity = await _context.SongsPlaylists
                        .Include(s => s.Song)
                        .Include(s => s.Playlist)
                        .FirstOrDefaultAsync(s => s.Id == selectedSong.Id);

                    if (selectedSongEntity != null)
                    {
                        selectedSong.PlayCount++;
                        _context.Entry(selectedSong).State = EntityState.Modified;

                        var applicationUser = await _context.ApplicationUsers
                            .Include(s => s.Room)
                            .FirstOrDefaultAsync(s => s.Id == applicationUserId);

                        var queuedSong = new SongQueued
                        {
                            ApplicationUser = applicationUser,
                            Room = applicationUser.Room,
                            Song = selectedSongEntity.Song,
                            SongRequestType = SongRequestType.Playlist,
                            WeightedValue = 0,
                        };

                        _context.SongsQueued.Add(queuedSong);

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            return _mapper.Map<SongQueuedVM>(queuedSong);
                        }
                    }
                }
            }
            return null;
        }
        public virtual async Task<SongPlaylistDTO[]> ReadSongsPlaylistAsync(Guid id)
        {
            var entities = await _context.SongsPlaylists
                .Where(s => s.Playlist.Id == id && s.Active)
                .ToListAsync();

            var dtos = new List<SongPlaylistDTO>();

            entities.ForEach(s => dtos.Add(_mapper.Map<SongPlaylistDTO>(s)));

            return dtos.ToArray();
        }
        public virtual async Task<SongPlaylistVM> ReadSongPlaylistAsync(Guid id)
        {
            var entity = await _context.SongsPlaylists
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<SongPlaylistVM>(entity) : null;
        }
        public virtual async Task<SongPlaylistVM> CreateSongPlaylistAsync(SongPlaylistCreateRequest request, Guid applicationUserId)
        {

            //var song = await _context.Songs
            //    .FirstOrDefaultAsync(s => s.YoutubeId == request.SongSearchResult.VideoId);

            //var playlist = await _context.Playlists
            //    .FirstOrDefaultAsync(s => s.Id == request.PlaylistId && s.ApplicationUserId == applicationUserId && s.Active);

            //if (playlist != null)
            //{
            //    if (songs != null)
            //    {
            //        // Create the Song
            //        YoutubeSearchResponse response;
            //        using (var httpClient = new HttpClient())
            //        {
            //            var url = $"https://www.googleapis.com/youtube/v3/videos?id={request.SongSearchResult.VideoId}&part=snippet";
            //            var url = $"https://www.googleapis.com/youtube/v3/videos?id={request.SongSearchResult.VideoId}&part=snippet&key={my-google-api-key}";
            //            var result = await httpClient.GetStringAsync(url);
            //            response = JsonConverter.DeserializeObject<YoutubeSearchResponse>(result);
            //        }

            //        var parsedLength = response.Items[0].ContentDetails.Duration.Substring(2);
            //        var hours = 0f;
            //        var minutes = 0f;
            //        var seconds = 0f;

            //        if (parsedLength.IndexOf("H") > 0)
            //        {
            //            if (float.TryParse(parsedLength.Substring(0, parsedLength.IndexOf("H")), out var hoursInt))
            //            {
            //                hours = hoursInt;
            //                parsedLength = parsedLength.Substring(parsedLength.IndexOf("H") + 1);
            //            }
            //        }

            //        if (parsedLength.IndexOf("M") > 0)
            //        {
            //            if (float.TryParse(parsedLength.Substring(0, parsedLength.IndexOf("M")), out var minutesInt))
            //            {
            //                minutes = minutesInt;
            //                parsedLength = parsedLength.Substring(parsedLength.IndexOf("M") + 1);
            //            }
            //        }

            //        if (parsedLength.IndexOf("S") > 0)
            //        {
            //            if (float.TryParse(parsedLength.Substring(0, parsedLength.IndexOf("S")), out var secondsInt))
            //            {
            //                seconds = secondsInt;
            //                parsedLength = parsedLength.Substring(parsedLength.IndexOf("S") + 1);
            //            }
            //        }

            //        var time = int.Parse(Math.Round((TimeSpan.FromHours(hours) + (TimeSpan.FromMinutes(minutes) + (TimeSpan.FromSeconds(seconds))));
            //        var songVM = await CreateSongAsync(new SongCreateRequest
            //        {
            //            YoutubeId = request.SongSearchResult.VideoId,
            //            SongLengthSeconds = time,
            //            SongName = request.SongSearchResult.SongName
            //        });

            //        song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == songVM.Id);
            //    }

            //    if (!await _context.SongsPlaylists.AnyAsync(s => s.SongId == song.Id && s.Active))
            //    {
            //        var songPlaylist = new SongPlaylist
            //        {
            //            Playlist = playlist,
            //            Song = song
            //        };

            //        _context.SongsPlaylists.Add(songPlaylist);
            //        if (await _context.SaveChangesAsync() > 0)
            //        {
            //            return _mapper.Map<SongPlaylistVM>(songPlaylist);
            //        }
            //    }
            //}
            //return null;

            var entity = _mapper.Map<SongPlaylist>(request);

            _context.SongsPlaylists.Add(entity);

            if (await _context.SaveChangesAsync() > 0)
            {
                return await ReadSongPlaylistAsync(entity.Id);
            }

            return null;
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

                _context.SongsQueued.Remove(queuedSongNext);

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

        public virtual async Task<YoutubeResults> SearchYoutubeLightAsync(string searchSnippet)
        {
            var youtubeResults = new YoutubeResults();

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Globals.GOOGLE_API_KEY,
                ApplicationName = "Listify"
            });

            var searchListRequest = youtubeService.Search.List(searchSnippet);
            searchListRequest.MaxResults = 20;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                //switch (searchResult.Id.Kind)
                //{
                //    case "youtube#video":
                //        videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                //        break;
                //}

                if (searchResult.Id.Kind == "youtube#video")
                {
                    youtubeResults.results.Add(new YoutubeResults.YoutubeResult
                    {
                        Id = new Guid(),
                        VideoId = searchResult.Id.VideoId,
                        SongName = searchResult.Snippet.Title,
                        //LengthSec = searchResult.
                    });
                }
            }
            return youtubeResults;
        }
        public virtual async Task<YoutubeResults> SearchYoutubeAsync(string searchSnippet)
        {
            var youtubeResults = new YoutubeResults();

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Globals.GOOGLE_API_KEY,
                ApplicationName = "Listify" 
            });

            var searchListRequest = youtubeService.Search.List(searchSnippet);
            searchListRequest.MaxResults = 20;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                //switch (searchResult.Id.Kind)
                //{
                //    case "youtube#video":
                //        videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                //        break;
                //}

                if (searchResult.Id.Kind == "youtube#video")
                {
                    youtubeResults.results.Add(new YoutubeResults.YoutubeResult
                    {
                        Id = new Guid(),
                        VideoId = searchResult.Id.VideoId,
                        SongName = searchResult.Snippet.Title,
                        //LengthSec = searchResult.
                    });
                }
            }
            return youtubeResults;
        }

        public async Task<ICollection<ApplicationUserRoomCurrencyVM>> AddCurrencyQuantityToAllUsersInRoomAsync(Guid roomId, Guid currencyId, int currencyQuantity, TransactionType transactionType)
        {
            var applicationUserRoomCurrencies = new List<ApplicationUserRoomCurrency>();

            // Get Room currency here
            var room = await _context.Rooms
                .Include(s => s.ApplicationUsersRooms)
                .FirstOrDefaultAsync(s => s.Id == roomId);

            var currency = await _context.Currencies
                .FirstOrDefaultAsync(s => s.Id == currencyId);

            currency.TimestampLastUpdated = DateTime.UtcNow;
            _context.Entry(currency).State = EntityState.Modified;

            if (room != null && currency != null)
            {
                foreach (var applicationUserRoom in room.ApplicationUsersRooms)
                {
                    var applicationUserRoomOnline = await _context.ApplicationUsersRoomsConnections
                            .Where(s => s.ApplicationUserRoomId == applicationUserRoom.Id && s.IsOnline)
                            .Select(s => s.ApplicationUserRoom)
                            .Distinct()
                            .FirstOrDefaultAsync();

                    if (applicationUserRoomOnline != null)
                    {
                        var applicationUserRoomCurrency = await _context.ApplicationUsersRoomsCurrencies
                            .Where(s => s.ApplicationUserRoomId == applicationUserRoomOnline.Id && s.Active && s.CurrencyId == currencyId)
                            .FirstOrDefaultAsync();

                        if (applicationUserRoomCurrency == null)
                        {
                            applicationUserRoomCurrency = new ApplicationUserRoomCurrency
                            {
                                ApplicationUserRoomId = applicationUserRoom.Id,
                                Currency = currency,
                                Quantity = 0,
                                TimeStamp = DateTime.UtcNow
                            };

                            _context.ApplicationUsersRoomsCurrencies.Add(applicationUserRoomCurrency);
                        }

                        if (!applicationUserRoomCurrencies.Any(s => s.Id == applicationUserRoomCurrency.Id))
                        {
                            applicationUserRoomCurrencies.Add(applicationUserRoomCurrency);
                        }
                    }
                }

                foreach (var applicationUserRoomCurrency in applicationUserRoomCurrencies)
                {
                    applicationUserRoomCurrency.Quantity += currencyQuantity;
                    _context.Entry(applicationUserRoomCurrency).State = EntityState.Modified;

                    var transaction = new Transaction
                    {
                        ApplicationUserRoomCurrencyId = applicationUserRoomCurrency.Id,
                        QuantityChange = currencyQuantity,
                        TransactionType = TransactionType.PollingCurrency,
                        TimeStamp = DateTime.UtcNow
                    };

                    _context.Add(transaction);
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    var vms = new List<ApplicationUserRoomCurrencyVM>();
                    applicationUserRoomCurrencies.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomCurrencyVM>(s)));
                    return vms;
                }
            }
            return null;
        }
        public virtual async Task<ICollection<ApplicationUserRoomConnectionVM>> PingApplicationUsersRoomsConnections()
        {
            var applicationUsersRoomsConnections = await _context.ApplicationUsersRoomsConnections
                .Where(s => s.Active)
                .ToListAsync();

            var connectionsToPing = new List<ApplicationUserRoomConnection>();
            //var connectionsRemoved = new List<ApplicationUserRoomConnectionVM>();

            foreach (var applicationUserRoomConnection in applicationUsersRoomsConnections)
            {
                try
                {
                    if (applicationUserRoomConnection.HasPingBeenSent || !applicationUserRoomConnection.IsOnline)
                    {
                        // Ping was not responded to - remove connection
                        _context.ApplicationUsersRoomsConnections.Remove(applicationUserRoomConnection);
                        //connectionsRemoved.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(applicationUserRoomConnection));
                    }
                    else
                    {
                        // Send Ping
                        applicationUserRoomConnection.HasPingBeenSent = true;
                        _context.Entry(applicationUserRoomConnection).State = EntityState.Modified;

                        connectionsToPing.Add(applicationUserRoomConnection);
                    }
                }
                catch
                { }
            }

            if (await _context.SaveChangesAsync() > 0 )
            {
                var vms = new List<ApplicationUserRoomConnectionVM>();
                connectionsToPing.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(s)));
                return vms;
                //return new PingPollVM
                //{
                //    ApplicationUserRoomConnectionsRemoved = connectionsRemoved,
                //    ApplicationUserRoomConnectionsToPing = connectionsToPing
                //};
            }

            return null;
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
