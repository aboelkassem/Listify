using Listify.Lib.VMs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using System.Net.Http;
using Listify.Paths;
using System.Linq;
using Listify.DAL;
using Listify.Lib.Requests;
using Listify.Domain.CodeFirst;
using Microsoft.EntityFrameworkCore;
using Listify.WebAPI.Models;
using AutoMapper;
using Listify.Lib.DTOs;
using Listify.BLL.Polls;
using Listify.BLL.Events.Args;
using System.Collections.Generic;
using Listify.Lib.Responses;
using Listify.Domain.Lib.Enums;
using System.Text;

namespace Listify.WebAPI.Hubs
{
    public class ListifyHub : Hub, IDisposable
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IHubContext<ListifyHub> _listifyHub;
        protected readonly IListifyServices _services;
        protected readonly IMapper _mapper;

        private static IPingPoll _pingPoll;

        public ListifyHub(
            ApplicationDbContext context,
            IHubContext<ListifyHub> listifyHub,
            IListifyServices services,
            IPingPoll pingPoll,
            IMapper mapper)
        {
            _context = context;
            _listifyHub = listifyHub;
            _services = services;
            _mapper = mapper;

            if (_pingPoll == null)
            {
                _pingPoll = pingPoll;
                _pingPoll.PollingEvent += async (s, e) =>
                {
                    await OnPingPollEvent(s, e);
                };
            }
        }

        protected virtual async Task OnPingPollEvent(object sender, PingPollEventArgs args)
        {
            foreach (var item in args.ConnectionsPinged)
            {
                await _listifyHub.Clients.Client(item.ConnectionId).SendAsync("PingRequest", "Ping");
            }

            //foreach (var applicationUserRoomConnection in args.PingPoll.ApplicationUserRoomConnectionsRemoved)
            //{
            //    await ForceServerDisconnectAsync(applicationUserRoomConnection.ConnectionId);
            //}
        }
        public async Task SendMessage(ChatMessageVM message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task RequestApplicationUserInformation()
        {
            try
            {
                var userId = await GetUserIdAsync();
                var applicationUser = await _services.ReadApplicationUserAsync(userId);

                await Clients.Caller.SendAsync("ReceiveApplicationUser", applicationUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task UpdateApplicationUserInformation(ApplicationUserUpdateRequest request)
        {
            try
            {
                var userId = await GetUserIdAsync();
                if (userId != Guid.Empty)
                {
                    var applicationUser = await _services.UpdateApplicationUserAsync(request, userId);
                    await Clients.Caller.SendAsync("ReceiveApplicationUserInformation", applicationUser);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RequestRooms()
        {
            try
            {
                var rooms = await _services.ReadRoomsAsync();
                await Clients.Caller.SendAsync("ReceiveRooms", rooms);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestRoom(string id)
        {
            try
            {
                RoomVM room;
                if (Guid.TryParse(id, out var guid))
                {
                    room = await _services.ReadRoomAsync(guid);
                    room.RoomKey = string.Empty;
                }
                else
                {
                    // this is the default room
                    var userId = await GetUserIdAsync();
                    var user = await _services.ReadApplicationUserAsync(userId);
                    room = await _services.ReadRoomAsync(user.Room.Id);

                    var decodedRoomKey = Encoding.UTF8.GetString(Convert.FromBase64String(room.RoomKey));

                    room.RoomKey = decodedRoomKey;
                }
                await Clients.Caller.SendAsync("ReceiveRoom", room);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        // for the owner/current user
        public async Task RequestPlaylists()
        {
            try
            {
                var userId = await GetUserIdAsync();

                if (userId != Guid.Empty)
                {
                    var playlists = await _services.ReadPlaylistsAsync(userId);
                    await Clients.Caller.SendAsync("ReceivePlaylists", playlists);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestPlaylist(string id)
        {
            try
            {
                if (Guid.TryParse(id, out var guid))
                {
                    var userId = await GetUserIdAsync();
                    if (userId != Guid.Empty)
                    {
                        var playlist = await _services.ReadPlaylistAsync(guid, userId);
                        playlist.SongsPlaylist = await _services.ReadSongsPlaylistAsync(playlist.Id);

                        await Clients.Caller.SendAsync("ReceivePlaylist", playlist);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task CreatePlaylist(PlaylistCreateRequest request)
        {
            try
            {
                var userId = await GetUserIdAsync();
                if (userId != Guid.Empty)
                {
                    // Create or update the playlist
                    PlaylistVM playlist = request.Id == Guid.Empty
                    ? await _services.CreatePlaylistAsync(request, userId)
                    : await _services.UpdatePlaylistAsync(request, userId);

                    await Clients.Caller.SendAsync("ReceivePlaylist", playlist);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task DeletePlaylist(string id)
        {
            try
            {
                if (Guid.TryParse(id, out var guid))
                {
                    var userId = await GetUserIdAsync();
                    if (userId != Guid.Empty)
                    {
                        if (await _services.DeletePlaylistAsync(guid, userId))
                        {
                            await Clients.Caller.SendAsync("ReceivePlaylists");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RequestSongsPlaylist(string id)
        {
            try
            {
                if (Guid.TryParse(id, out var guid))
                {
                    var SongsPlaylist = await _services.ReadSongsPlaylistAsync(guid);
                    await Clients.Caller.SendAsync("ReceiveSongsPlaylist", SongsPlaylist);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestSongPlaylist(string id)
        {
            try
            {
                if (Guid.TryParse(id, out var guid))
                {
                    //var userId = await GetUserIdAsync();

                    var songPlaylist = await _services.ReadSongPlaylistAsync(guid);
                    await Clients.Caller.SendAsync("ReceiveSongPlaylist", songPlaylist);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task CreateSongPlaylist(SongPlaylistCreateRequest request)
        {
            try
            {
                var userId = await GetUserIdAsync();
                if (userId != Guid.Empty)
                {
                    // Create or update the SongPlaylist
                    var songPlaylist = await _services.CreateSongPlaylistAsync(request, userId);
                    await Clients.Caller.SendAsync("ReceiveSongPlaylist", songPlaylist);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task DeleteSongPlaylist(string id)
        {
            try
            {
                if (Guid.TryParse(id, out var guid))
                {
                    var userId = await GetUserIdAsync();
                    if (userId != Guid.Empty)
                    {
                        var songPlaylist = await _services.ReadSongPlaylistAsync(guid);
                        if (songPlaylist != null && await _services.DeleteSongPlaylistAsync(guid, userId))
                        {
                            var songsPlaylist = await _services.ReadSongsPlaylistAsync(songPlaylist.Playlist.Id);
                            await Clients.Caller.SendAsync("ReceiveSongsPlaylist", songsPlaylist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RequestSearchYoutube(string searchSnippet)
        {
            try
            {
                var responses = await _services.SearchYoutubeLightAsync(searchSnippet);
                await Clients.Caller.SendAsync("ReceiveSearchYoutube", responses);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestSearchYoutubePlaylist(string searchSnippet)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestAddYoutubePlaylistToPlaylist(string youtubePlaylistUrl, Guid playlistId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestAddSpotifyPlaylistToPlaylist(string spotifyPlaylistId, Guid playlistId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //public async Task RequestCurrencies()
        //{
        //    try
        //    {
        //        //var userId = await GetUserIdAsync();
        //        var currencies = await _services.ReadCurrenciesAsync();
        //        await Clients.Caller.SendAsync("ReceiveCurrencies", currencies);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //public async Task RequestCurrency(string id)
        //{
        //    try
        //    {
        //        if (Guid.TryParse(id, out var guid))
        //        {
        //            //var userId = await GetUserIdAsync();

        //            var currency = await _services.ReadCurrencyAsync(guid);
        //            await Clients.Caller.SendAsync("ReceiveCurrency", currency);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //public async Task CreateCurrency(CurrencyCreateRequest request)
        //{
        //    try
        //    {
        //        var userId = await GetUserIdAsync();
        //        if (userId != Guid.Empty)
        //        {
        //            var currency = request.Id == Guid.Empty
        //                ? await _services.CreateCurrencyAsync(request, userId)
        //                : await _services.UpdateCurrencyAsync(request, userId);
        //            await Clients.Caller.SendAsync("ReceiveCurrency", currency);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //public async Task DeleteCurrency(string id)
        //{
        //    try
        //    {
        //        if (Guid.TryParse(id, out var guid))
        //        {
        //            if (await _services.DeleteCurrencyAsync(guid))
        //            {
        //                await Clients.Caller.SendAsync("ReceiveCurrency");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        public async Task RequestCurrenciesRoom(string roomId)
        {
            try
            {
                if (Guid.TryParse(roomId, out var guid))
                {
                    var currenciesRoom = await _services.ReadCurrenciesRoomAsync(guid);

                    await Clients.Caller.SendAsync("ReceiveCurrenciesRoom", currenciesRoom);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestCurrencyRoom(string id)
        {
            try
            {
                if (Guid.TryParse(id, out var guid))
                {
                    var currencyRoom = await _services.ReadCurrencyAsync(guid);

                    await Clients.Caller.SendAsync("ReceiveCurrencyRoom", currencyRoom);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RequestPurchasableItems()
        {
            try
            {
                var purchasableItems = await _services.ReadPurchasableItemsAsync();
                await Clients.Caller.SendAsync("ReceivePurchasableItems", purchasableItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestPurchasableItem(Guid id)
        {
            try
            {
                var purchasableItem = await _services.ReadPurchasableItemAsync(id);
                await Clients.Caller.SendAsync("ReceivePurchasableItem", purchasableItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task CreatePurchasableItem(PurchasableItemCreateRequest request)
        {
            try
            {
                var purchasableItem = request.Id == Guid.Empty
                    ? await _services.CreatePurchasableItemAsync(request)
                    : await _services.UpdatePurchasableItemAsync(request);

                await Clients.Caller.SendAsync("ReceivePurchasableItem", purchasableItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task DeletePurchasableItem(string id)
        {
            try
            {
                var userId = await GetUserIdAsync();
                if (Guid.TryParse(id, out var guid))
                {
                    if (await _services.DeletePurchasableItemAsync(guid, userId))
                    {
                        await Clients.Caller.SendAsync("ReceivePurchasableItem");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task CreatePurchase(PurchaseCreateRequest request)
        {
            try
            {
                var userId = await GetUserIdAsync();
                var purchase = await _services.CreatePurchaseAsync(request, userId);
                await Clients.Caller.SendAsync("ReceivePurchase", purchase);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestAuthToLockedRoom(string roomKey, Guid roomId)
        {
            try
            {
                var room = await _services.ReadRoomAsync(roomId);

                await Clients.Caller.SendAsync("ResponseAuthToLockedRoom", new AuthToLockedRoomResponse
                {
                    AuthToLockedRoomResponseType = await _services.CheckAuthToLockedRoomAsync(roomKey, roomId)
                        ? AuthToLockedRoomResponseType.Success
                        : AuthToLockedRoomResponseType.Fail,
                    Room = room
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task PingResponse()
        {
            var connection = await _services.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

            if (connection != null)
            {
                try
                {
                    await _services.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
                    {
                        Id = connection.Id,
                        HasPingBeenSent = false,
                        IsOnline = true
                    });
                }
                catch
                {
                }
            }
        }

        //public async Task CreateComment(CommentCreateRequest request)
        //{

        //}
        //public async Task RequestReleaseNotes()
        //{

        //}

        //public async Task QueuePlaylistInRoomHome(Guid playlistId)
        //{
        //    try
        //    {
        //        var applicationUserRoomConnection = await _services.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);
        //        if (applicationUserRoomConnection == null)
        //        {
        //            applicationUserRoomConnection = await CheckConnectionAsync();

        //            if (applicationUserRoomConnection == null)
        //            {
        //                await _listifyHub.Clients.Client(Context.ConnectionId).SendAsync("ForceServerDisconnect");
        //            }
        //        }

        //        var applicationUserRoom = await _services.ReadApplicationUserRoomAsync(applicationUserRoomConnection.ApplicationUserRoom.Id);
        //        var applicationUser = await _services.ReadApplicationUserAsync(applicationUserRoom.ApplicationUser.Id);
        //        var roomHub = await _services.ReadRoomAsync(applicationUser.Room.Id);

        //        var songsQueued = await _services.QueuePlaylistInRoomHomeAsync(playlistId, applicationUser.Id);

        //        await Clients.Caller.SendAsync("RecieveQueuePlaylistInRoomHome", songsQueued);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //public async Task RequestPurchases()
        //{

        //}

        public override async Task OnConnectedAsync()
        {
            try
            {
                var context = Context.GetHttpContext();
                var token = context.Request.Query["token"];

                if (!string.IsNullOrWhiteSpace(token))
                {
                    //var userInfoClient = new IdentityModel.Client.UserInfoClient();
                    var client = new HttpClient();

                    var disco = await client.GetDiscoveryDocumentAsync(Globals.IDENTITY_SERVER_AUTHORITY_URL);
                    var response = await client.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = disco.UserInfoEndpoint,
                        Token = token
                    });

                    var username = response.Claims.ToList().First(s => s.Type == "name").Value;
                    var userId = response.Claims.ToList().First(s => s.Type == "preferred_username").Value;

                    var applicationUser = await _services.ReadApplicationUserAsync(userId);

                    if (applicationUser == null)
                    {
                        var roomCodeNew = username;
                        // this prevents 2 rooms from having the same code
                        var index = 0;
                        while (await _context.Rooms.AnyAsync(s => s.RoomCode.Trim().ToLower() == roomCodeNew.Trim().ToLower()))
                        {
                            roomCodeNew = username + index++;
                        }

                        // this prevents 2 users from having the same username
                        var i = 0;
                        while (await _context.ApplicationUsers.AnyAsync(s => s.Username.Trim().ToLower() == username.Trim().ToLower()))
                        {
                            username += i++;
                        }

                        // the room is attached here
                        applicationUser = await _services.CreateApplicationUserAsync(new ApplicationUserCreateRequest
                        {
                            AspNetUserId = userId,
                            Username = username,
                            RoomCode = roomCodeNew
                        });
                    }

                    // if the room was not specified, then get the default
                    var room = await _services.ReadRoomAsync(applicationUser.Room.Id);

                    if (room != null)
                    {
                        var applicationUserRoom = await _services.ReadApplicationUserRoomAsync(applicationUser.Id, room.Id);

                        if (applicationUserRoom == null)
                        {
                            applicationUserRoom = await _services.CreateApplicationUserRoomAsync(new ApplicationUserRoomCreateRequest
                            {
                                IsOnline = true,
                                RoomId = room.Id
                            }, applicationUser.Id);
                        }

                        await _services.CheckCurrenciesRoomAsync(room.Id);

                        var connection = await _services.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

                        connection = connection == null
                            ? await _services.CreateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionCreateRequest
                            {
                                ApplicationUserRoomId = applicationUserRoom.Id,
                                ConnectionId = Context.ConnectionId,
                                IsOnline = true
                            })
                            : await _services.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
                            {
                                HasPingBeenSent = connection.HasPingBeenSent,
                                IsOnline = true,
                                Id = connection.Id
                            });

                        await base.OnConnectedAsync();

                        var applicationUserVM = _mapper.Map<ApplicationUserVM>(applicationUser);

                        await Clients.Caller.SendAsync("ReceiveApplicationUser", applicationUserVM);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            var connection = await _services.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);
            if (connection != null)
            {
                connection = await _services.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
                {
                    Id = connection.Id,
                    HasPingBeenSent = connection.HasPingBeenSent,
                    IsOnline = false
                });
            }
        }

        protected virtual async Task<Guid> GetUserIdAsync()
        {
            var applicationUserRoomConnection = await _services.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

            if (applicationUserRoomConnection == null)
            {
                applicationUserRoomConnection = await CheckConnectionAsync();

                if (applicationUserRoomConnection == null)
                {
                    await _listifyHub.Clients.Client(Context.ConnectionId).SendAsync("ForceServerDisconnect");
                    return Guid.Empty;
                }
            }

            var applicationUserRoom = await _services.ReadApplicationUserRoomAsync(applicationUserRoomConnection.ApplicationUserRoom.Id);
            return applicationUserRoom.ApplicationUser.Id;
        }
        protected virtual async Task<ApplicationUserRoomConnectionVM> CheckConnectionAsync()
        {
            var context = Context.GetHttpContext();
            var token = context.Request.Query["token"];

            //var userInfoClient = new IdentityModel.Client.UserInfoClient();
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(Globals.IDENTITY_SERVER_AUTHORITY_URL);
            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = token
            });

            var username = response.Claims.ToList().First(s => s.Type == "name").Value;
            var userId = response.Claims.ToList().First(s => s.Type == "preferred_username").Value;

            if (!string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(userId))
            {
                var applicationUser = await _services.ReadApplicationUserAsync(userId);

                if (applicationUser == null)
                {
                    var roomCodeNew = username;
                    // this prevents 2 rooms from having the same code
                    var index = 0;
                    while (await _context.Rooms.AnyAsync(s => s.RoomCode.Trim().ToLower() == roomCodeNew.Trim().ToLower()))
                    {
                        roomCodeNew = username + index++;
                    }

                    // the room is attached here
                    applicationUser = await _services.CreateApplicationUserAsync(new ApplicationUserCreateRequest
                    {
                        AspNetUserId = userId,
                        Username = username,
                        RoomCode = roomCodeNew
                    });
                }

                // if the room was not specified, then get the default
                var room = await _services.ReadRoomAsync(applicationUser.Room.Id);

                if (room != null)
                {
                    room = await _services.UpdateRoomAsync(new RoomUpdateRequest
                    {
                        Id = room.Id,
                        RoomCode = room.RoomCode,
                        IsRoomPublic = true,
                        IsRoomOnline = true
                    });

                    var applicationUserRoom = await _services.ReadApplicationUserRoomAsync(applicationUser.Id, room.Id);

                    if (applicationUserRoom == null)
                    {
                        applicationUserRoom = await _services.CreateApplicationUserRoomAsync(new ApplicationUserRoomCreateRequest
                        {
                            IsOnline = true,
                            RoomId = room.Id
                        }, applicationUser.Id);
                    }

                    var connection = await _services.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

                    connection = connection == null
                    ? await _services.CreateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionCreateRequest
                    {
                        ApplicationUserRoomId = applicationUserRoom.Id,
                        ConnectionId = Context.ConnectionId,
                        IsOnline = true
                    })
                    : await _services.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
                    {
                        HasPingBeenSent = connection.HasPingBeenSent,
                        IsOnline = true,
                        Id = connection.Id
                    });

                    return connection;
                }
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (_pingPoll != null)
            {
                _pingPoll.PollingEvent -= async (s, e) => await OnPingPollEvent(s, e);
            }

            base.Dispose(disposing);
        }
    }
}
