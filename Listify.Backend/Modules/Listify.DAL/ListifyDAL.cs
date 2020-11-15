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
using System.Web;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace Listify.DAL
{
    public class ListifyDAL : IListifyDAL
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;
        protected IServiceScopeFactory _serviceScopeFactory;

        public ListifyDAL(
            ApplicationDbContext context,
            IServiceScopeFactory serviceScopeFactory,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public virtual async Task<ApplicationUserVM> ReadApplicationUserAsync(Guid id)
        {
            var entity = await _context.ApplicationUsers
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);
            if (entity != null)
            {

                if (entity.DateJoined == DateTime.MinValue)
                {
                    entity.DateJoined = DateTime.UtcNow;
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                var playlists = await _context.Playlists
                    .Where(s => s.ApplicationUserId == entity.Id && s.Active)
                    .ToListAsync();

                var usersOnline = await _context.ApplicationUsersRooms
                    .Where(s => s.RoomId == entity.Room.Id && s.IsOnline && s.Active)
                    .CountAsync();

                if (entity.Room.IsRoomLocked)
                {
                    if (entity.Room.RoomKey != null && IsBase64String(entity.Room.RoomKey))
                    {
                        entity.Room.RoomKey = Encoding.UTF8.GetString(Convert.FromBase64String(entity.Room.RoomKey));
                    }
                    //else
                    //{
                    //    entity.Room.RoomKey = string.Empty;
                    //}
                }

                var follows = await _context.Follows
                    .Include(s => s.ApplicationUser)
                    .Where(s => s.RoomId == entity.Room.Id && s.Active)
                    .ToListAsync();

                var roomGenres = await _context.RoomsGenres
                    .Include(s => s.Genre)
                    .Where(s => s.RoomId == entity.Room.Id && s.Active)
                    .ToListAsync();

                var vm = _mapper.Map<ApplicationUserVM>(entity);
                vm.Room.NumberUsersOnline = usersOnline;
                return vm;
            }
            return null;
        }
        public virtual async Task<ApplicationUserVM> ReadApplicationUserAsync(string aspNetUserId)
        {
            var entity = await _context.ApplicationUsers
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.AspNetUserId == aspNetUserId && s.Active);

            if (entity != null)
            {
                if (entity.DateJoined == DateTime.MinValue)
                {
                    entity.DateJoined = DateTime.UtcNow;
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                var playlists = await _context.Playlists
                    .Where(s => s.ApplicationUserId == entity.Id && s.Active)
                    .ToListAsync();

                var usersOnline = await _context.ApplicationUsersRooms
                    .Where(s => s.RoomId == entity.Room.Id && s.IsOnline && s.Active)
                    .CountAsync();

                //if (entity.Room.IsRoomLocked)
                //{
                //    if (entity.Room.RoomKey != null && IsBase64String(entity.Room.RoomKey))
                //    {
                //        entity.Room.RoomKey = Encoding.UTF8.GetString(Convert.FromBase64String(entity.Room.RoomKey));
                //    }
                //    else
                //    {
                //        entity.Room.RoomKey = string.Empty;
                //    }
                //}

                var follows = await _context.Follows
                    .Include(s => s.ApplicationUser)
                    .Where(s => s.RoomId == entity.Room.Id && s.Active)
                    .ToListAsync();

                var roomGenres = await _context.RoomsGenres
                    .Include(s => s.Genre)
                    .Where(s => s.RoomId == entity.Room.Id && s.Active)
                    .ToListAsync();

                var vm = _mapper.Map<ApplicationUserVM>(entity);
                vm.Room.NumberUsersOnline = usersOnline;
                return vm;
            }
            return null;
        }
        public virtual async Task<ApplicationUserVM> CreateApplicationUserAsync(ApplicationUserCreateRequest request)
        {
            var entity = _mapper.Map<ApplicationUser>(request);

            await _context.ApplicationUsers.AddAsync(entity);

            if (entity.DateJoined == DateTime.MinValue)
            {
                entity.DateJoined = DateTime.UtcNow;
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            var room = new Room
            {
                ApplicationUser = entity,
                RoomCode = request.Username,
                RoomTitle = request.Username
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

            var playlist = new Playlist
            {
                ApplicationUser = entity,
                IsSelected = true,
                IsPublic = false,
                PlaylistName = "Default Playlist"
            };
            await _context.Playlists.AddAsync(playlist);

            var introSong = await _context.Songs
                .FirstOrDefaultAsync(s => s.YoutubeId == "owQ5YZrleGU");

            var songPlaylist = new SongPlaylist
            {
                PlayCount = 0,
                Playlist = playlist,
                SongRequestType = SongRequestType.Playlist,
                Song = introSong
            };
            await _context.SongsPlaylists.AddAsync(songPlaylist);

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

            if (entity.DateJoined == DateTime.MinValue)
            {
                entity.DateJoined = DateTime.UtcNow;
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            if (entity != null)
            {
                string encodedRoomKey = string.Empty;
                if (!string.IsNullOrWhiteSpace(request.RoomKey))
                {
                    encodedRoomKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.RoomKey));
                }

                entity.Username = request.Username;
                entity.ChatColor = request.ChatColor;
                entity.ProfileTitle = request.ProfileTitle;
                entity.ProfileDescription = request.ProfileDescription;
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


                var roomGenres = await _context.RoomsGenres
                    .Where(s => s.RoomId == entity.Room.Id && s.Active)
                    .ToListAsync();

                foreach (var roomGenre in roomGenres)
                {
                    roomGenre.Active = false;
                    _context.Entry(roomGenre).State = EntityState.Modified;
                }

                foreach (var roomGenre in request.RoomGenres)
                {
                    if (roomGenres.Any(s => s.GenreId == roomGenre.Genre.Id))
                    {
                        var roomGenreSelected = roomGenres.First(s => s.GenreId == roomGenre.Genre.Id);
                        roomGenreSelected.Active = true;
                        roomGenreSelected.TimeStamp = DateTime.UtcNow;

                        _context.Entry(roomGenreSelected).State = EntityState.Modified;
                    }
                    else
                    {
                        var genreEntity = await _context.Genres
                            .FirstOrDefaultAsync(s => s.Id == roomGenre.Genre.Id && s.Active);

                        if (genreEntity != null)
                        {
                            entity.Room.RoomGenres.Add(new RoomGenre
                            {
                                GenreId = genreEntity.Id,
                                RoomId = entity.Id
                            });
                        }
                    }
                }

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

        public virtual async Task<ProfileVM> ReadProfileAsync(string username)
        {
            var entity = await _context.ApplicationUsers
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Username.Trim().ToLower() == username.Trim().ToLower() && s.Active);

            if (entity != null)
            {
                if (entity.DateJoined == DateTime.MinValue)
                {
                    entity.DateJoined = DateTime.UtcNow;
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                entity.Room.RoomKey = string.Empty;

                var follows = await _context.Follows
                    .Include(s => s.ApplicationUser)
                    .Where(s => s.RoomId == entity.Room.Id && s.Active)
                    .ToListAsync();

                var roomGenres = await _context.RoomsGenres
                    .Include(s => s.Genre)
                    .Where(s => s.RoomId == entity.Room.Id && s.Active)
                    .ToListAsync();

                var vm = _mapper.Map<ProfileVM>(entity);
                vm.NumberFollows = follows.Count;

                var playlists = await _context.Playlists
                    .Where(s => s.ApplicationUserId == entity.Id && s.IsPublic && s.Active)
                    .ToListAsync();

                foreach (var playlist in entity.Playlists)
                {
                    var playlistGenres = await _context.PlaylistsGenres
                        .Include(s => s.Genre)
                        .Where(s => s.PlaylistId == playlist.Id && s.Active)
                        .ToListAsync();

                    var playlistVM = _mapper.Map<PlaylistVM>(playlist);

                    playlistVM.NumberOfSongs = await _context.SongsPlaylists
                        .Where(s => s.PlaylistId == playlist.Id && s.Active)
                        .CountAsync();

                    vm.Playlists.Add(playlistVM);
                }
                return vm;
            }
            return null;
        }
        public virtual async Task<ProfileVM> UpdateProfileAsync(ProfileUpdateRequest request, Guid applicationUserId)
        {
            var entity = await _context.ApplicationUsers
                .Include(s => s.Room)
                .Include(s => s.Playlists)
                .FirstOrDefaultAsync(s => s.Id == applicationUserId);

            if (entity != null)
            {
                entity.Username = request.Username;
                entity.ProfileTitle = request.ProfileTitle;
                entity.ProfileImageUrl = request.ProfileImageUrl;
                entity.ProfileDescription = request.ProfileDescription;
                _context.Entry(entity).State = EntityState.Modified;
            }

            return await _context.SaveChangesAsync() > 0 ? _mapper.Map<ProfileVM>(entity) : null;
        }

        public virtual async Task<ApplicationUserRoomVM> ReadApplicationUserRoomAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRooms
                .Include(s => s.ApplicationUser)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            var numberFollows = await _context.Follows
                .Where(s => s.RoomId == entity.RoomId && s.Active)
                .CountAsync();

            var usersOnline = await _context.ApplicationUsersRooms
                .Where(s => s.RoomId == entity.RoomId && s.IsOnline && s.Active)
                .CountAsync();

            //  entity.Room.RoomKey = string.Empty;

            var vm = _mapper.Map<ApplicationUserRoomVM>(entity);
            vm.Room.NumberFollows = numberFollows;
            vm.Room.NumberUsersOnline = usersOnline;

            return entity != null ? vm : null;
        }
        public virtual async Task<ApplicationUserRoomVM> ReadApplicationUserRoomOwnerAsync(Guid roomId)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(Globals.DEV_CONNECTION_STRING);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var entity = await context.ApplicationUsersRooms
                        .Include(s => s.ApplicationUser)
                        .Include(s => s.Room)
                        .FirstOrDefaultAsync(s => s.RoomId == roomId && s.IsOwner && s.Active);

                    var numberFollows = await context.Follows
                        .Where(s => s.RoomId == entity.RoomId && s.Active)
                        .CountAsync();

                    var usersOnline = await context.ApplicationUsersRooms
                        .Where(s => s.RoomId == entity.RoomId && s.IsOnline && s.Active)
                        .CountAsync();

                    // entity.Room.RoomKey = string.Empty;

                    var vm = _mapper.Map<ApplicationUserRoomVM>(entity);
                    vm.Room.NumberFollows = numberFollows;
                    vm.Room.NumberUsersOnline = usersOnline;

                    return entity != null ? vm : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public virtual async Task<ApplicationUserRoomVM[]> ReadApplicationUsersRoomOnlineAsync(string connectionId)
        {
            var applicationUserRoomConneciton = await _context.ApplicationUsersRoomsConnections
                .Include(s => s.ApplicationUserRoom)
                .FirstOrDefaultAsync(s => s.ConnectionId == connectionId && s.IsOnline && s.Active);

            if (applicationUserRoomConneciton != null)
            {
                var applicationUserRoom = await _context.ApplicationUsersRooms
                    .Include(s => s.Room)
                    .FirstOrDefaultAsync(s => s.Id == applicationUserRoomConneciton.ApplicationUserRoom.Id && s.Active);

                if (applicationUserRoom != null)
                {
                    var applicationUsersRoom = await _context.ApplicationUsersRooms
                        .Include(s => s.ApplicationUser)
                        .Include(s => s.Room)
                        .Where(s => s.RoomId == applicationUserRoom.RoomId &&
                            s.IsOnline && s.Active)
                        .ToListAsync();

                    var vms = new List<ApplicationUserRoomVM>();

                    foreach (var applicationUserRoomEntity in applicationUsersRoom)
                    {
                        var vm = _mapper.Map<ApplicationUserRoomVM>(applicationUserRoomEntity);
                        vms.Add(vm);
                    }

                    return vms.ToArray();
                }
            }
            return null;
        }
        public virtual async Task<ApplicationUserRoomVM> ReadApplicationUserRoomAsync(Guid applicationUserId, Guid roomId)
        {
            try
            {
                var entity = await _context.ApplicationUsersRooms
                    .Include(s => s.ApplicationUser)
                    .Include(s => s.Room)
                    .FirstOrDefaultAsync(s => s.ApplicationUserId == applicationUserId &&
                        s.RoomId == roomId && s.Active);

                if (entity != null)
                {
                    var numberFollows = await _context.Follows
                    .Where(s => s.RoomId == entity.RoomId && s.Active)
                    .CountAsync();

                    var usersOnline = await _context.ApplicationUsersRooms
                        .Where(s => s.RoomId == entity.RoomId && s.IsOnline && s.Active)
                        .CountAsync();

                    //if (entity.Room.RoomKey != null)
                    //{
                    //    entity.Room.RoomKey = string.Empty;
                    //}

                    var vm = _mapper.Map<ApplicationUserRoomVM>(entity);
                    vm.Room.NumberFollows = numberFollows;
                    vm.Room.NumberUsersOnline = usersOnline;

                    return vm;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
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
                entity.TimeStamp = DateTime.UtcNow;
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
                .FirstOrDefaultAsync(s => s.Id == applicationUserRoomId && s.Active);

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
                            Quantity = 20   // the starting currency per room
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
                .Include(s => s.ApplicationUserRoom)
                .Include(s => s.ApplicationUserRoom.Room)
                .Include(s => s.ApplicationUserRoom.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<ChatMessageVM>(entity) : null;
        }
        public virtual async Task<ChatMessageVM[]> ReadChatMessagesAsync(Guid roomId)
        {
            var entities = await _context.ChatMessages
                .Include(s => s.ApplicationUserRoom)
                .Include(s => s.ApplicationUserRoom.Room)
                .Include(s => s.ApplicationUserRoom.ApplicationUser)
                .Where(s => s.ApplicationUserRoom.Room.Id == roomId && s.Active)
                .OrderByDescending(s => s.TimeStamp)
                .Take(30)
                .ToListAsync();

            var vms = new List<ChatMessageVM>();

            entities.ForEach(s => vms.Add(_mapper.Map<ChatMessageVM>(s)));

            return entities != null ? vms.ToArray() : null;
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
                //await Task.Run(async () =>
                //{
                //    using (var scope = _serviceScopeFactory.CreateScope())
                //    {
                //        var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

                //        var connections = await db.ApplicationUsersRoomsConnections
                //            .Include(s => s.ApplicationUserRoom)
                //            .Where(s => s.ApplicationUserRoom.RoomId == roomId &&
                //                s.IsOnline && s.Active && s.ApplicationUserRoom.Active)
                //            .ToListAsync();

                //        var vms = new List<ApplicationUserRoomConnectionVM>();
                //        connections.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(s)));

                //        return vms.ToArray();
                //    }
                //});
                //using (var context = new ApplicationDbContext())
                //{
                //var connections = await context.ApplicationUsersRoomsConnections
                //        .Include(s => s.ApplicationUserRoom)
                //        .Where(s => s.ApplicationUserRoom.RoomId == roomId &&
                //            s.IsOnline && s.Active && s.ApplicationUserRoom.Active)
                //        .ToListAsync();

                //    var vms = new List<ApplicationUserRoomConnectionVM>();
                //    connections.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(s)));

                //    return vms.ToArray();
                //}

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(Globals.DEV_CONNECTION_STRING);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var connections = await context.ApplicationUsersRoomsConnections
                            .Include(s => s.ApplicationUserRoom)
                            .Where(s => s.ApplicationUserRoom.RoomId == roomId &&
                                s.IsOnline && s.Active && s.ApplicationUserRoom.Active)
                            .ToListAsync();

                    var vms = new List<ApplicationUserRoomConnectionVM>();
                    connections.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(s)));

                    return vms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ApplicationUserRoomConnectionVM>().ToArray();
            }
        }
        public virtual async Task<ApplicationUserRoomConnectionVM> ReadApplicationUserRoomConnectionAsync(Guid id)
        {
            var entity = await _context.ApplicationUsersRoomsConnections
                .Include(s => s.ApplicationUserRoom)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsOnline && s.Active);

            return entity != null ? _mapper.Map<ApplicationUserRoomConnectionVM>(entity) : null;
        }
        public virtual async Task<ApplicationUserRoomConnectionVM[]> ReadApplicationUserRoomConnectionByApplicationUserRoomIdAsync(Guid applicationUserRoomId)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(Globals.DEV_CONNECTION_STRING);

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                var entities = await context.ApplicationUsersRoomsConnections
                    .Include(s => s.ApplicationUserRoom)
                    .Where(s => s.ApplicationUserRoomId == applicationUserRoomId && s.IsOnline && s.Active)
                    .ToListAsync();

                var vms = new List<ApplicationUserRoomConnectionVM>();
                entities.ForEach(s => vms.Add(_mapper.Map<ApplicationUserRoomConnectionVM>(s)));

                return vms.ToArray();
            }
        }
        public virtual async Task<ApplicationUserRoomConnectionVM> ReadApplicationUserRoomConnectionAsync(string connectionId)
        {
            var entity = await _context.ApplicationUsersRoomsConnections
                .Include(s => s.ApplicationUserRoom)
                .FirstOrDefaultAsync(s => s.ConnectionId == connectionId && s.IsOnline && s.Active);

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
                entity.IsOnline = request.IsOnline;
                entity.TimeStamp = DateTime.UtcNow;
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
        public virtual async Task<PlaylistVM[]> ReadPlaylistsAsync(Guid applicationUserId)
        {
            var entities = await _context.Playlists
                .Include(s => s.PlaylistGenres)
                .Include(s => s.ApplicationUser)
                .Where(s => s.ApplicationUserId == applicationUserId && s.Active)
                .ToListAsync();

            foreach (var entity in entities)
            {
                var playlistGenres = await _context.PlaylistsGenres
                    .Include(s => s.Genre)
                    .Where(s => s.PlaylistId == entity.Id && s.Active)
                    .ToListAsync();
            }

            var vms = new List<PlaylistVM>();
            entities.ForEach(s => vms.Add(_mapper.Map<PlaylistVM>(s)));
            return vms.ToArray();
        }
        public virtual async Task<PlaylistVM> ReadPlaylistSelectedAsync(Guid applicationUserId)
        {
            var entity = await _context.Playlists
                .Include(s => s.ApplicationUser)
                .Include(s => s.SongsPlaylist)
                .Include(s => s.PlaylistGenres)
                .FirstOrDefaultAsync(s => s.ApplicationUserId == applicationUserId && s.IsSelected && s.Active);

            if (entity != null)
            {
                var songsPlaylists = await _context.SongsPlaylists
                    .Include(s => s.Song)
                    .Where(s => s.PlaylistId == entity.Id && s.Active)
                    .ToListAsync();

                return _mapper.Map<PlaylistVM>(entity);
            }
            return null;
        }
        public virtual async Task<PlaylistVM> ReadPlaylistAsync(Guid id, Guid applicationUserId)
        {
            var entity = await _context.Playlists
                .Include(s => s.PlaylistGenres)
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                var songsPlaylsit = await _context.SongsPlaylists
                    .Include(s => s.Song)
                    .Where(s => s.PlaylistId == entity.Id && s.Active)
                    .ToListAsync();

                var playlistGenres = await _context.PlaylistsGenres
                    .Include(s => s.Genre)
                    .Where(s => s.PlaylistId == entity.Id && s.Active)
                    .ToListAsync();

                return _mapper.Map<PlaylistVM>(entity);
            }

            return null;
        }
        public virtual async Task<PlaylistVM> CreatePlaylistAsync(PlaylistCreateRequest request, Guid applicationUserId)
        {
            var user = await _context.ApplicationUsers
                .Include(s => s.Playlists)
                .FirstOrDefaultAsync(s => s.Id == applicationUserId && s.Active);

            if (user.Playlists.Count == 0 || user.Playlists.Where(s => s.Active).ToList().Count < user.PlaylistCountMax)
            {
                var entity = _mapper.Map<Playlist>(request);

                foreach (var playlistGenre in request.PlaylistGenres)
                {
                    var genre = await _context.Genres
                        .FirstOrDefaultAsync(s => s.Id == playlistGenre.Genre.Id && s.Active);

                    if (genre != null)
                    {
                        entity.PlaylistGenres.Add(new PlaylistGenre
                        {
                            Playlist = entity,
                            Genre = genre
                        });
                    }
                }

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
                .Include(s => s.PlaylistGenres)
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.ApplicationUserId == applicationUserId && s.Active);

            if (entity != null)
            {
                entity.PlaylistGenres.Clear();

                foreach (var playlistGenre in request.PlaylistGenres)
                {
                    var genre = await _context.Genres
                        .FirstOrDefaultAsync(s => s.Id == playlistGenre.Genre.Id && s.Active);

                    if (genre != null)
                    {
                        entity.PlaylistGenres.Add(new PlaylistGenre
                        {
                            Playlist = entity,
                            Genre = genre
                        });
                    }
                }

                entity.PlaylistName = request.PlaylistName;
                entity.IsSelected = request.IsSelected;
                entity.IsPublic = request.IsPublic;
                entity.TimeStamp = DateTime.UtcNow;
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

                var playlistGenres = await _context.PlaylistsGenres
                    .Where(s => s.PlaylistId == entity.Id && s.Active)
                    .ToListAsync();

                foreach (var playlistGenre in playlistGenres)
                {
                    playlistGenre.Active = false;
                    _context.Entry(playlistGenre).State = EntityState.Modified;
                }

                var songsPlaylist = await _context.SongsPlaylists
                    .Where(s => s.PlaylistId == entity.Id && s.Active)
                    .ToListAsync();

                foreach (var songPlaylist in songsPlaylist)
                {
                    songPlaylist.Active = false;
                    _context.Entry(songPlaylist).State = EntityState.Modified;
                }

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
        public virtual async Task<SongVM> ReadSongAsync(string youtubeId)
        {
            try
            {
                var entity = await _context.Songs
                    .FirstOrDefaultAsync(s => s.YoutubeId == youtubeId && s.Active);

                return entity != null ? _mapper.Map<SongVM>(entity) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public virtual async Task<SongVM> CreateSongAsync(string youtubeId)
        {
            try
            {
                // Create the Song
                YoutubeSearchResponse response;
                using (var httpClient = new HttpClient())
                {
                    var url = $"https://www.googleapis.com/youtube/v3/videos?id={youtubeId}&part=snippet&part=contentDetails&key={Globals.GOOGLE_API_KEY}";
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
                return await CreateSongAsync(new SongCreateRequest
                {
                    YoutubeId = response.Items.FirstOrDefault().Id,
                    SongLengthSeconds = time,
                    SongName = response.Items.FirstOrDefault().Snippet.Title,
                    ThumbnailUrl = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Url,
                    ThumbnailWidth = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Width,
                    ThumbnailHeight = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Height
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public virtual async Task<SongVM> CreateSongAsync(YoutubeResults.YoutubeResult songSearchResult)
        {
            try
            {
                // Create the Song
                YoutubeSearchResponse response;
                using (var httpClient = new HttpClient())
                {
                    var url = $"https://www.googleapis.com/youtube/v3/videos?id={songSearchResult.VideoId}&part=snippet&part=contentDetails&key={Globals.GOOGLE_API_KEY}";
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
                return await CreateSongAsync(new SongCreateRequest
                {
                    YoutubeId = songSearchResult.VideoId,
                    SongLengthSeconds = time,
                    SongName = songSearchResult.SongName,
                    ThumbnailUrl = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Url,
                    ThumbnailWidth = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Width,
                    ThumbnailHeight = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Height
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public virtual async Task<SongVM> CreateSongAsync(Google.Apis.YouTube.v3.Data.PlaylistItem playlistItem)
        {
            try
            {
                var parsedLength = playlistItem.ContentDetails.EndAt.Substring(2);
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

                return await CreateSongAsync(new SongCreateRequest
                {
                    SongName = playlistItem.Snippet.Title,
                    SongLengthSeconds = time,
                    YoutubeId = playlistItem.ContentDetails.VideoId,
                    ThumbnailUrl = playlistItem.Snippet.Thumbnails.Default__.Url,
                    ThumbnailHeight = playlistItem.Snippet.Thumbnails.Default__.Height,
                    ThumbnailWidth = playlistItem.Snippet.Thumbnails.Default__.Width,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public virtual async Task<SongVM> CreateSongAsync(Google.Apis.YouTube.v3.Data.SearchResult songSearchResult)
        {
            try
            {
                return await CreateSongAsync(new SongCreateRequest
                {
                    SongName = songSearchResult.Snippet.Title,
                    SongLengthSeconds = await GetTimeOfYoutubeVideoBySeconds(songSearchResult.Id.VideoId),
                    YoutubeId = songSearchResult.Id.VideoId,
                    ThumbnailUrl = songSearchResult.Snippet.Thumbnails.Default__.Url,
                    ThumbnailHeight = songSearchResult.Snippet.Thumbnails.Default__.Height,
                    ThumbnailWidth = songSearchResult.Snippet.Thumbnails.Default__.Width,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
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
                entity.TimeStamp = DateTime.UtcNow;
                _context.Entry(entity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadSongAsync(entity.Id);
                }
            }
            return null;
        }
        public virtual async Task<SongVM> UpdateSongAsync(string youtubeId)
        {
            try
            {
                // Create the Song
                YoutubeSearchResponse response;
                using (var httpClient = new HttpClient())
                {
                    var url = $"https://www.googleapis.com/youtube/v3/videos?id={youtubeId}&part=snippet&part=contentDetails&key={Globals.GOOGLE_API_KEY}";
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

                var songEntity = await _context.Songs
                    .FirstOrDefaultAsync(s => s.YoutubeId == youtubeId);

                songEntity.SongLengthSeconds = time;
                songEntity.SongName = response.Items.FirstOrDefault().Snippet.Title;
                songEntity.ThumbnailUrl = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Url;
                songEntity.ThumbnailWidth = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Width;
                songEntity.ThumbnailHeight = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Height;
                _context.Entry(songEntity).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadSongAsync(songEntity.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
        public virtual async Task<int> GetTimeOfYoutubeVideoBySeconds(string youtubeId)
        {
            try
            {
                // Create the Song
                YoutubeSearchResponse response;
                using (var httpClient = new HttpClient())
                {
                    var url = $"https://www.googleapis.com/youtube/v3/videos?id={youtubeId}&part=snippet&part=contentDetails&key={Globals.GOOGLE_API_KEY}";
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
                return time;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public virtual async Task<SongQueuedVM[]> QueuePlaylistInRoomHomeAsync(Guid playlistId, bool isRandomized, Guid applicationUserId)
        {
            try
            {
                var applicationUser = await _context.ApplicationUsers
                    .FirstOrDefaultAsync(s => s.Id == applicationUserId && s.Active);

                var room = await _context.Rooms
                    .FirstOrDefaultAsync(s => s.ApplicationUserId == applicationUserId && s.Active);

                var playlist = await _context.Playlists
                    .FirstOrDefaultAsync(s => s.Id == playlistId && s.Active);

                if (room != null && playlist != null)
                {
                    var songsQueued = await _context.SongsQueued
                        .Where(s => s.RoomId == room.Id &&
                            !s.HasBeenPlayed &&
                            !s.HasBeenSkipped &&
                            s.Active)
                        .ToListAsync();

                    var songsPlaylsit = await _context.SongsPlaylists
                        .Where(s => s.PlaylistId == playlist.Id && s.Active)
                        .ToListAsync();

                    if (isRandomized)
                    {
                        songsPlaylsit = songsPlaylsit.OrderBy(s => Guid.NewGuid()).ToList();
                    }

                    var counter = songsQueued.Count();

                    foreach (var songPlaylist in songsPlaylsit)
                    {
                        if (counter < applicationUser.QueueSongsCount && !songsQueued.Any(s => s.SongId == songPlaylist.SongId))
                        {
                            counter++;

                            await _context.SongsQueued.AddAsync(new SongQueued
                            {
                                ApplicationUserId = applicationUserId,
                                RoomId = room.Id,
                                SongId = songPlaylist.SongId,
                                WeightedValue = 0
                            });
                        }
                    }
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return (await ReadSongsQueuedAsync(room.Id)).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public virtual async Task<SongQueuedVM> QueueSongPlaylistNext(Guid applicationUserId)
        {
            var playlist = await _context.Playlists
                .Include(s => s.SongsPlaylist)
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

                //var songPlaylist = await _context.SongsPlaylists
                //    .Include(s => s.Song)
                //    .Include(s => s.Playlist)
                //    .Where(s => s.PlaylistId == playlist.Id && s.Active)
                //    .OrderBy(s => Guid.NewGuid())
                //    .FirstOrDefaultAsync();

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
                        SongRequestType = SongRequestType.Queued,
                        WeightedValue = 0,
                    };

                    //await _context.SongsQueued.AddAsync(queuedSong);

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
                        var url = $"https://www.googleapis.com/youtube/v3/videos?id={request.SongSearchResult.VideoId}&part=snippet&part=contentDetails&key={Globals.GOOGLE_API_KEY}";
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
                        SongName = request.SongSearchResult.SongName,
                        ThumbnailUrl = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Url,
                        ThumbnailWidth = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Width,
                        ThumbnailHeight = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Height
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
                        Song = song,
                        SongRequestType = SongRequestType.Playlist,
                        PlaylistId = playlist.Id,
                        SongId = song.Id
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
                .Where(s => s.RoomId == roomId && !s.HasBeenPlayed && !s.HasBeenSkipped && s.Active)
                .OrderByDescending(s => s.WeightedValue)
                .ThenBy(s => s.TimeStamp)
                .ToListAsync();

            var vms = new List<SongQueuedVM>();

            entities.ForEach(s => vms.Add(_mapper.Map<SongQueuedVM>(s)));

            return vms.ToArray();
        }
        public virtual async Task<SongQueuedVM> ReadSongQueuedAsync(Guid id)
        {
            var entity = await _context.SongsQueued
                .Include(s => s.Song)
                .Include(s => s.Room)
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return _mapper.Map<SongQueuedVM>(entity);
        }
        public virtual async Task<SongQueuedVM> CreateSongQueuedAsync(SongQueuedCreateRequest request)
        {

            // Validation
            if (request.SongSearchResult.QuantityWagered <= 0)
            {
                return null;
            }

            var applicationUserRoomCurrency = await _context.ApplicationUsersRoomsCurrenciesRooms
                .Include(s => s.CurrencyRoom)
                .Include(s => s.CurrencyRoom.Currency)
                .Include(s => s.ApplicationUserRoom)
                .FirstOrDefaultAsync(s => s.Id == request.SongSearchResult.ApplicationUserRoomCurrencyId && s.Active);

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
                            var url = $"https://www.googleapis.com/youtube/v3/videos?id={request.SongSearchResult.VideoId}&part=snippet&part=contentDetails&key={Globals.GOOGLE_API_KEY}";
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
                            SongName = request.SongSearchResult.SongName,
                            ThumbnailUrl = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Url,
                            ThumbnailWidth = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Width,
                            ThumbnailHeight = response.Items.FirstOrDefault().Snippet.Thumbnails.Default.Height
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
                            SongRequestType = SongRequestType.Queued,
                            WeightedValue = request.SongSearchResult.QuantityWagered * applicationUserRoomCurrency.CurrencyRoom.Currency.Weight,
                            TransactionsSongQueued = new List<TransactionSongQueued>
                            {
                                new TransactionSongQueued
                                {
                                    ApplicationUserRoomCurrencyId = applicationUserRoomCurrency.Id,
                                    QuantityChanged = request.SongSearchResult.QuantityWagered,
                                    TransactionType = TransactionType.Request
                                }
                            }
                        };
                        await _context.SongsQueued.AddAsync(songQueued);

                        if (applicationUserRoomCurrency != null)
                        {
                            applicationUserRoomCurrency.Quantity -= request.SongSearchResult.QuantityWagered;
                            _context.Entry(applicationUserRoomCurrency).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        songQueued.WeightedValue += request.SongSearchResult.QuantityWagered * applicationUserRoomCurrency.CurrencyRoom.Currency.Weight;
                        songQueued.TransactionsSongQueued.Add(new TransactionSongQueued
                        {
                            TransactionType = TransactionType.Upvote,
                        });

                        _context.Entry(songQueued).State = EntityState.Modified;
                    }

                    if (applicationUserRoomCurrency != null)
                    {
                        //applicationUserRoomCurrency.Quantity -= request.SongSearchResult.QuantityWagered;
                        //_context.Entry(applicationUserRoomCurrency).State = EntityState.Modified;

                        if (applicationUserRoomCurrency.Quantity >= 0 && await _context.SaveChangesAsync() > 0)
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
                .Where(s => s.RoomId == roomId &&
                    !s.HasBeenPlayed &&
                    !s.HasBeenSkipped &&
                    s.Room.ApplicationUserId == applicationUserId && s.Active)
                .OrderByDescending(s => s.WeightedValue)
                .ThenBy(s => s.TimeStamp)
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
                        QuantityChanged = request.Quantity,
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
                entity.TimeStamp = DateTime.UtcNow;
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

        public virtual async Task<RoomVM[]> ReadRoomsAsync()
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(Globals.DEV_CONNECTION_STRING);

                // ToDo: need to solve this exception
                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var entities = await context.Rooms
                        .Include(s => s.ApplicationUser)
                        .Where(s => s.IsRoomOnline && s.IsRoomPublic && s.Active)
                        .ToListAsync();

                    var vms = new List<RoomVM>();

                    foreach (var entity in entities)
                    {
                        var usersOnline = await context.ApplicationUsersRooms
                                .Where(s => s.RoomId == entity.Id && s.IsOnline && s.Active)
                                .CountAsync();

                        var follows = await context.Follows
                            .Include(s => s.ApplicationUser)
                            .Where(s => s.RoomId == entity.Id && s.Active)
                            .ToListAsync();

                        var roomGenres = await context.RoomsGenres
                            .Include(s => s.Genre)
                            .Where(s => s.RoomId == entity.Id && s.Active)
                            .ToListAsync();

                        var vm = _mapper.Map<RoomVM>(entity);
                        vm.NumberUsersOnline = usersOnline;
                        vms.Add(vm);
                    }

                    return vms.OrderBy(s => s.NumberUsersOnline).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<RoomVM>().ToArray();
            }
        }
        public virtual async Task<RoomVM[]> ReadRoomsFollowsAsync(Guid applicationUserId)
        {
            var entities = await _context.Follows
                    .Include(s => s.Room)
                    .Where(s => s.ApplicationUserId == applicationUserId && s.Active)
                    .ToListAsync();

            var vms = new List<RoomVM>();

            foreach (var entity in entities.Where(s => s.Room.IsRoomOnline && s.Room.IsRoomPublic))
            {
                vms.Add(await ReadRoomAsync(entity.RoomId));
            }

            return vms.OrderBy(s => s.NumberUsersOnline).ToArray();
        }
        public virtual async Task<RoomVM> ReadRoomAsync(Guid id)
        {
            var entity = await _context.Rooms
                .Include(s => s.CurrenciesRoom)
                .Include(s=> s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (entity != null)
            {
                var currenciesRoom = await _context.CurrenciesRooms
                    .Where(s => s.RoomId == entity.Id && s.Active)
                    .ToListAsync();

                var follows = await _context.Follows
                    .Include(s => s.ApplicationUser)
                    .Where(s => s.RoomId == entity.Id && s.Active)
                    .ToListAsync();

                var roomGenres = await _context.RoomsGenres
                    .Include(s => s.Genre)
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
                .Include(s => s.CurrenciesRoom)
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.RoomCode.Trim().ToLower() == roomCode.Trim().ToLower() && s.Active);
            
            if (entity != null)
            {
                var currenciesRoom = await _context.CurrenciesRooms
                    .Where(s => s.RoomId == entity.Id && s.Active)
                    .ToListAsync();

                var follows = await _context.Follows
                    .Include(s => s.ApplicationUser)
                    .Where(s => s.RoomId == entity.Id && s.Active)
                    .ToListAsync();

                var roomGenres = await _context.RoomsGenres
                    .Include(s => s.Genre)
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
                entity.IsRoomOnline = request.IsRoomOnline;
                entity.IsRoomPlaying = request.IsRoomPlaying;
                entity.TimeStamp = DateTime.UtcNow;

                var roomGenres = await _context.RoomsGenres
                    .Where(s => s.RoomId == entity.Id && s.Active)
                    .ToListAsync();

                foreach (var roomGenre in roomGenres)
                {
                    roomGenre.Active = false;
                    _context.Entry(roomGenre).State = EntityState.Modified;
                }

                foreach (var roomGenre in request.RoomGenres)
                {
                    if (roomGenres.Any(s => s.GenreId == roomGenre.Genre.Id))
                    {
                        var roomGenreSelected = roomGenres.First(s => s.GenreId == roomGenre.Genre.Id);
                        roomGenreSelected.Active = true;
                        roomGenreSelected.TimeStamp = DateTime.UtcNow;

                        _context.Entry(roomGenreSelected).State = EntityState.Modified;
                    }
                    else
                    {
                        var genreEntity = await _context.Genres
                            .FirstOrDefaultAsync(s => s.Id == roomGenre.Genre.Id && s.Active);

                        if (genreEntity != null)
                        {
                            entity.RoomGenres.Add(new RoomGenre
                            {
                                GenreId = genreEntity.Id,
                                RoomId = entity.Id
                            });
                        }
                    }
                }

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

                var roomGenres = await _context.RoomsGenres
                    .Where(s => s.RoomId == entity.Id && s.Active)
                    .ToListAsync();

                foreach (var roomGenre in roomGenres)
                {
                    roomGenre.Active = false;
                    _context.Entry(roomGenre).State = EntityState.Modified;
                }

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
            entity.TimeStamp = DateTime.UtcNow;
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

        public virtual async Task<SongVM> SearchYoutubeAndReturnFirstResultAsync(string searchSnippet)
        {
            try
            {
                var songSearchResult = await SearchYoutubeAsync(searchSnippet);
                var firstSearchResult = songSearchResult.Results.FirstOrDefault();
                if (firstSearchResult != null)
                {
                    var songVM = await ReadSongAsync(firstSearchResult.VideoId);

                    return songVM != null ? songVM : await CreateSongAsync(firstSearchResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                                SongName = searchResult.Snippet.Title,
                                YoutubeThumbnailUrl = searchResult.Snippet.Thumbnails.Default__.Url,
                                YoutubeThumbnailHeight = searchResult.Snippet.Thumbnails.Default__.Height,
                                YoutubeThumbnailWidth = searchResult.Snippet.Thumbnails.Default__.Width,
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
        // return first relevance result
        public virtual async Task<YoutubeResults> SearchYoutubeAsync(string searchSnippet)
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
                searchListRequest.MaxResults = 1;

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
                                SongName = searchResult.Snippet.Title,
                                YoutubeThumbnailUrl = searchResult.Snippet.Thumbnails.Default__.Url,
                                YoutubeThumbnailHeight = searchResult.Snippet.Thumbnails.Default__.Height,
                                YoutubeThumbnailWidth = searchResult.Snippet.Thumbnails.Default__.Width,
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

        public virtual async Task<ICollection<ApplicationUserRoomCurrencyRoomVM>> AddCurrencyQuantityToAllUsersInRoomAsync(Guid roomId, Guid currencyRoomId, int currencyQuantity, TransactionType transactionType)
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

                            //var transaction = new Transaction
                            //{
                            //    ApplicationUserRoomCurrencyId = entity.Id,
                            //    QuantityChange = currencyQuantity,
                            //    TransactionType = TransactionType.PollingCurrency,
                            //    TimeStamp = DateTime.UtcNow
                            //};

                            //await _context.Transactions.AddAsync(transaction);
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
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(Globals.DEV_CONNECTION_STRING);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
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

                        //var isRoomOnline = true;

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
                                        // remove all connections that has been pinged(ended) every 5 minutes
                                        if ((DateTime.UtcNow - applicationUserRoomConnection.TimeStamp).TotalMinutes > 5)
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

                            //applicationUserRoom.IsOnline = await context.ApplicationUsersRoomsConnections
                            //    .AnyAsync(s => s.ApplicationUserRoomId == applicationUserRoom.Id &&
                            //        s.IsOnline && s.Active);

                            //context.Entry(applicationUserRoom).State = EntityState.Modified;
                        }

                        //var ownerRooms = await context.ApplicationUsersRooms
                        //    .Where(s => s.RoomId == room.Id &&
                        //        s.IsOwner &&
                        //        s.Active)
                        //    .ToListAsync();

                        //if (ownerRooms.Count > 0)
                        //{
                        //    foreach (var ownerRoom in ownerRooms)
                        //    {
                        //        if (!await context.ApplicationUsersRoomsConnections
                        //            .Where(s => s.ApplicationUserRoomId == ownerRoom.Id &&
                        //                s.IsOnline &&
                        //                s.Active)
                        //            .AnyAsync())
                        //        {
                        //            isRoomOnline = false;
                        //        }
                        //    }
                        //}

                        //room.IsRoomOnline = isRoomOnline;
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

        public virtual async Task<PlaylistVM[]> ReadPlaylistsCommunityAsync()
        {
            var playlistsPublic = await _context.Playlists
                .Include(s => s.ApplicationUser)
                .Include(s => s.PlaylistGenres)
                .Include(s => s.SongsPlaylist)
                .Where(s => s.IsPublic && s.Active)
                .ToListAsync();

            var vms = new List<PlaylistVM>();

            foreach (var playlistPublic in playlistsPublic)
            {
                var playlistsGenres = await _context.PlaylistsGenres
                    .Include(s => s.Genre)
                    .Where(s => s.PlaylistId == playlistPublic.Id && s.Active)
                    .ToListAsync();

                var songsPlaylistCount = await _context.SongsPlaylists
                    .Include(s => s.Song)
                    .Where(s => s.PlaylistId == playlistPublic.Id && s.Active)
                    .CountAsync();

                if (songsPlaylistCount > 0)
                {
                    var vm = _mapper.Map<PlaylistVM>(playlistPublic);
                    vm.NumberOfSongs = songsPlaylistCount;
                    vms.Add(vm);
                }
            }

            return vms.ToArray();
        }
        public virtual async Task<GenreDTO[]> ReadGenresAsync()
        {
            var entities = await _context.Genres
                .OrderBy(s => s.Name)
                .Where(s => s.Active)
                .ToListAsync();

            var dtos = new List<GenreDTO>();

            entities.ForEach(s => dtos.Add(_mapper.Map<GenreDTO>(s)));

            return dtos.ToArray();
        }

        public virtual async Task<FollowVM[]> ReadFollowsByRoomIdAsync(Guid roomId)
        {
            var follows = await _context.Follows
                .Include(s => s.ApplicationUser)
                .Where(s => s.RoomId == roomId && s.Active)
                .ToListAsync();

            var vms = new List<FollowVM>();

            follows.ForEach(s => vms.Add(_mapper.Map<FollowVM>(s)));

            return vms.ToArray();
        }
        public virtual async Task<FollowVM[]> ReadFollowsByApplicationUserIdAsync(Guid applicationUserId)
        {
            var follows = await _context.Follows
                .Include(s => s.ApplicationUser)
                .Where(s => s.ApplicationUserId == applicationUserId && s.Active)
                .ToListAsync();

            var vms = new List<FollowVM>();

            follows.ForEach(s => vms.Add(_mapper.Map<FollowVM>(s)));

            return vms.ToArray();
        }
        public virtual async Task<FollowVM> ReadFollowAsync(Guid id)
        {
            var entity = await _context.Follows
                .Include(s => s.ApplicationUser)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == id && s.Active);

            return entity != null ? _mapper.Map<FollowVM>(entity) : null;
        }
        public virtual async Task<FollowVM> ReadFollowAsync(Guid roomId, Guid applicationUserId)
        {
            var entity = await _context.Follows
                .Include(s => s.ApplicationUser)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.RoomId == roomId && s.ApplicationUserId == applicationUserId && s.Active);

            return entity != null ? _mapper.Map<FollowVM>(entity) : null;
        }
        public virtual async Task<FollowVM> CreateFollowAsync(FollowCreateRequest request, Guid applicationUserId)
        {
            var entity = await _context.Follows
                .FirstOrDefaultAsync(s => s.ApplicationUserId == applicationUserId &&
                    s.RoomId == request.RoomId &&
                    s.Active);

            var applicationUser = await _context.ApplicationUsers
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == applicationUserId && s.Active);

            if (entity == null && applicationUser != null && request.RoomId != applicationUser.Room.Id)
            {
                entity = _mapper.Map<Follow>(request);
                entity.ApplicationUserId = applicationUserId;
                await _context.Follows.AddAsync(entity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await ReadFollowAsync(entity.Id);
                }
            }
            return null;
        }
        public virtual async Task<bool> DeleteFollowAsync(Guid followId, Guid applicationUserId)
        {
            var entity = await _context.Follows
                .FirstOrDefaultAsync(s => s.Id == followId && s.ApplicationUserId == applicationUserId && s.Active);

            if (entity != null)
            {
                _context.Follows.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public virtual async Task<SongPlaylistVM[]> AddSongsToPlaylistAsync(SongVM[] songs, Guid playlistId, Guid applicationUserId)
        {
            try
            {
                var applicationUser = await _context.ApplicationUsers
                .FirstOrDefaultAsync(s => s.Id == applicationUserId && s.Active);

                var playlist = await _context.Playlists
                    .FirstOrDefaultAsync(s => s.Id == playlistId && s.ApplicationUserId == applicationUserId && s.Active);

                if (playlist != null)
                {
                    var songsPlaylist = await _context.SongsPlaylists
                        .Where(s => s.PlaylistId == playlist.Id && s.Active)
                        .ToListAsync();

                    var counter = songsPlaylist.Count();

                    foreach (var song in songs)
                    {
                        if (applicationUser.PlaylistSongCount > counter && !songsPlaylist.Any(s => s.SongId == song.Id))
                        {
                            // Add the song
                            // ToDo: Continue Implementation of this method
                            var songVM = await ReadSongAsync(song.YoutubeId);

                            if (songVM == null)
                            {
                                var songNew = await CreateSongAsync(new SongCreateRequest
                                {
                                    YoutubeId = song.YoutubeId,
                                    SongName = song.SongName,
                                    SongLengthSeconds = await GetTimeOfYoutubeVideoBySeconds(song.YoutubeId),
                                    ThumbnailUrl = song.ThumbnailUrl,
                                    ThumbnailHeight = song.ThumbnailHeight,
                                    ThumbnailWidth = song.ThumbnailWidth
                                });

                                await _context.SongsPlaylists.AddAsync(new SongPlaylist
                                {
                                    PlaylistId = playlist.Id,
                                    SongId = songNew.Id,
                                    PlayCount = 0,
                                    SongRequestType = SongRequestType.Playlist,
                                });
                            }
                            else
                            {
                                await _context.SongsPlaylists.AddAsync(new SongPlaylist
                                {
                                    PlaylistId = playlist.Id,
                                    SongId = songVM.Id,
                                    PlayCount = 0,
                                    SongRequestType = SongRequestType.Playlist,
                                });
                            }
                        }
                    }

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return await ReadSongsPlaylistAsync(playlist.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public virtual async Task<SongPlaylistVM[]> AddYoutubePlaylistToPlaylistAsync(string youtubePlaylistUrl, Guid playlistId, Guid applicationUserId)
        {
            try
            {
                // segment URL to get playlistId from playlist URL
                var playlistUrl = new Uri(youtubePlaylistUrl);
                var youtubePlaylistId = HttpUtility.ParseQueryString(playlistUrl.Query).Get("list");

                // get the songs of the playlist from api
                YoutubePlaylistSearchResponse response;
                using (var httpClient = new HttpClient())
                {
                    var url = $"https://www.googleapis.com/youtube/v3/playlistItems?playlistId={youtubePlaylistId}&part=snippet&maxResults=50&key={Globals.GOOGLE_API_KEY}";
                    var result = await httpClient.GetStringAsync(url);
                    response = JsonConvert.DeserializeObject<YoutubePlaylistSearchResponse>(result);
                }

                var songsPlaylist = response.Items;
                var songs = new List<SongVM>();

                foreach (var songPlaylist in songsPlaylist)
                {
                    if (songPlaylist.Snippet != null)
                    {
                        var song = await ReadSongAsync(songPlaylist.Snippet.ResourceId.VideoId);
                        if (song != null)
                        {
                            songs.Add(song);
                        }
                        else
                        {
                            // create the public songVM and add it to the list
                            if (songPlaylist.Snippet.Title != "Private video")
                            {
                                var songVM = new SongVM
                                {
                                    SongName = songPlaylist.Snippet.Title,
                                    YoutubeId = songPlaylist.Snippet.ResourceId.VideoId,
                                    SongLengthSeconds = await GetTimeOfYoutubeVideoBySeconds(songPlaylist.Snippet.ResourceId.VideoId),
                                    ThumbnailUrl = songPlaylist.Snippet.Thumbnails.Default.Url,
                                    ThumbnailHeight = songPlaylist.Snippet.Thumbnails.Default.Height,
                                    ThumbnailWidth = songPlaylist.Snippet.Thumbnails.Default.Width
                                };
                                songs.Add(songVM);
                            }
                        }
                    }
                }

                if (songs.Count > 0)
                {
                    return await AddSongsToPlaylistAsync(songs.ToArray(), playlistId, applicationUserId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public virtual async Task<bool> ClearSongsQueuedAsync(Guid roomId)
        {
            // this needs to refund all the transactions that were applied to the song

            var songsQueued = await _context.SongsQueued
                .Include(s => s.TransactionsSongQueued)
                .Where(s => s.RoomId == roomId &&
                    !s.HasBeenPlayed &&
                    !s.HasBeenSkipped &&
                    s.Active)
                .ToListAsync();

            foreach (var songQueued in songsQueued)
            {
                songQueued.Active = false;
                _context.Entry(songQueued).State = EntityState.Modified;

                foreach (var transaction in songQueued.TransactionsSongQueued)
                {
                    var applicationUserRoomCurrencyRoom = await _context.ApplicationUsersRoomsCurrenciesRooms
                        .FirstOrDefaultAsync(s => s.Id == transaction.ApplicationUserRoomCurrencyId);

                    applicationUserRoomCurrencyRoom.Quantity += transaction.QuantityChanged;
                    _context.Entry(applicationUserRoomCurrencyRoom).State = EntityState.Modified;
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }
        public virtual async Task<bool> AddCurrentSongQueuedToDefaultPlaylist(Guid songQueuedId, Guid applicationUserId)
        {
            var applicationUser = await _context.ApplicationUsers
                .FirstOrDefaultAsync(s => s.Id == applicationUserId && s.Active);

            var defaultPlaylist = await _context.Playlists
                .FirstOrDefaultAsync(s => s.ApplicationUserId == applicationUserId && s.IsSelected && s.Active);

            var songsPlaylist = await _context.SongsPlaylists
                .Include(s => s.Song)
                .Include(s => s.Playlist)
                .Where(s => s.PlaylistId == defaultPlaylist.Id && s.Active)
                .ToListAsync();

            var songQueued = await _context.SongsQueued
                .Include(s => s.Song)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == songQueuedId);

            if (songQueued != null)
            {
                var counter = songsPlaylist.Count();

                if (applicationUser.PlaylistSongCount > counter && !songsPlaylist.Any(s => s.SongId == songQueued.SongId))
                {
                    await _context.SongsPlaylists.AddAsync(new SongPlaylist
                    {
                        PlaylistId = defaultPlaylist.Id,
                        SongId = songQueued.Song.Id,
                        SongRequestType = SongRequestType.Playlist,
                        PlayCount = 0
                    });
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }

            return false;
        }
        //public virtual async Task<bool> UpvoteSongQueuedNoWager(Guid songQueuedId)
        //{
        //    var songQueued = await _context.SongsQueued
        //        .Include(s => s.Room)
        //        .FirstOrDefaultAsync(s => s.Id == songQueuedId && s.Active);

        //    if (songQueued != null)
        //    {
        //        var songsQueued = await _context.SongsQueued
        //            .Where(s => s.RoomId == songQueued.Room.Id &&
        //                !s.HasBeenPlayed &&
        //                !s.HasBeenSkipped &&
        //                s.Active)
        //            .OrderByDescending(s => s.WeightedValue)
        //            .ThenBy(s => s.TimeStamp)
        //            .ToListAsync();

        //        for (int i = 0; i < songsQueued.Count; i++)
        //        {
        //            if (songsQueued[i].Id == songQueued.Id)
        //            {
        //                var selectedSongTimestamp = songsQueued[i].TimeStamp;
        //            }
        //        }
        //    }
        //}
        //public virtual async Task<bool> DownvoteSongQueuedNoWager(Guid songQueuedId)
        //{
        //    throw new NotImplementedException();
        //}
        public virtual async Task<ApplicationUserVM> UpdateApplicationUserProfileImageUrlAsync(string profileImageUrl, Guid profileId, Guid applicationUserId)
        {
            throw new NotImplementedException();
        }
        public virtual async Task<ApplicationUserVM> UpdateApplicationUserRoomImageUrlAsync(string roomImageUrl, Guid roomId, Guid applicationUserId)
        {
            throw new NotImplementedException();
        }
        public virtual async Task<ApplicationUserVM> UpdatePlaylistImageUrlAsync(string playlistImageUrl, Guid playlistId, Guid applicationUserId)
        {
            throw new NotImplementedException();
        }
        public virtual async Task<bool> ClearUserProfileImageAsync(Guid applicationUserId)
        {
            throw new NotImplementedException();
        }
        public virtual async Task<bool> ClearRoomImageAsync(Guid roomId, Guid applicationUserId)
        {
            throw new NotImplementedException();
        }
        public virtual async Task<bool> ClearPlaylistImageAsync(Guid playlistId, Guid applicationUserId)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<ApplicationUserRoomCurrencyRoomVM[]> SkipSongAsync(Guid songQueuedId)
        {
            var songQueued = await _context.SongsQueued
                .FirstOrDefaultAsync(s => s.Id == songQueuedId);

            if (songQueued != null)
            {
                songQueued.HasBeenSkipped = true;
                songQueued.TimestampSkipped = DateTime.UtcNow;
                songQueued.TimeStamp = DateTime.UtcNow;
                _context.Entry(songQueued).State = EntityState.Modified;

                var transactions = await _context.TransactionsSongsQueued
                    .Where(s => s.SongQueuedId == songQueuedId &&
                        !s.HasBeenRefunded &&
                        s.Active)
                    .ToListAsync();
                
                var guids = new List<Guid>();
                
                foreach (var transaction in transactions)
                {
                    var applicationUserRoomCurrencyRoom = await _context.ApplicationUsersRoomsCurrenciesRooms
                        .Include(s => s.ApplicationUserRoom)
                        .FirstOrDefaultAsync(s => s.Id == transaction.ApplicationUserRoomCurrencyId);

                    if (applicationUserRoomCurrencyRoom != null)
                    {
                        applicationUserRoomCurrencyRoom.Quantity += transaction.QuantityChanged;
                        _context.Entry(applicationUserRoomCurrencyRoom).State = EntityState.Modified;

                        transaction.HasBeenRefunded = true;
                        transaction.RefundTimestamp = DateTime.UtcNow;
                        _context.Entry(transaction).State = EntityState.Modified;

                        guids.Add(applicationUserRoomCurrencyRoom.Id);
                    }
                }
                if (await _context.SaveChangesAsync() > 0)
                {
                    var vms = new List<ApplicationUserRoomCurrencyRoomVM>();

                    foreach (var guid in guids)
                    {
                        vms.Add(await ReadApplicationUserRoomCurrencyRoomAsync(guid));
                    }

                    return vms.ToArray();
                }
            }
            return new ApplicationUserRoomCurrencyRoomVM[0];
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

        private bool IsBase64String(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        public virtual async void Dispose()
        {
        }
    }
}
