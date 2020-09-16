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
using Listify.Services;

namespace Listify.WebAPI.Hubs
{
    public class RoomHub : Hub, IDisposable
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IHubContext<RoomHub> _roomHub;
        protected readonly IListifyDAL _dal;
        protected readonly IListifyService _service;
        protected readonly IMapper _mapper;

        private static IPingPoll _pingPoll;
        private static ICurrencyPoll _currencyPoll;

        public RoomHub(
            ApplicationDbContext context,
            IHubContext<RoomHub> roomHub,
            IListifyDAL dal,
            IListifyService service,
            IPingPoll pingPoll,
            ICurrencyPoll currencyPoll,
            IMapper mapper)
        {
            _context = context;
            _roomHub = roomHub;
            _dal = dal;
            _service = service;
            _mapper = mapper;

            if (_pingPoll == null)
            {
                _pingPoll = pingPoll;
                _pingPoll.PollingEvent += async (s, e) => await OnPingPollEvent(s, e);
            }

            if (_currencyPoll == null)
            {
                _currencyPoll = currencyPoll;
                _currencyPoll.PollingEvent += async (s, e) => await OnCurrencyPollEvent(s, e);
            }
        }

        protected virtual async Task OnPingPollEvent(object sender, PingPollEventArgs args)
        {
            foreach (var item in args.ConnectionsPinged)
            {
                await _roomHub.Clients.Client(item.ConnectionId).SendAsync("PingRequest", "Ping");
            }

            //foreach (var applicationUserRoomConnection in args.PingPoll.ApplicationUserRoomConnectionsRemoved)
            //{
            //    await ForceServerDisconnectAsync(applicationUserRoomConnection.ConnectionId);
            //}
        }
        protected virtual async Task OnCurrencyPollEvent(object sender, CurrencyPollEventArgs args)
        {
            // have a ping service, get the connections out of the database that match the room
            var connections = await _dal.ReadApplicationUsersRoomsConnectionsAsync(args.Room.Id);

            foreach (var connection in connections.Where(s => s.IsOnline))
            {
                try
                {
                    if (args.ApplicationUserRoomsCurrencies.Any(s => s.ApplicationUserRoom.Id == connection.ApplicationUserRoom.Id))
                    {
                        var applicationUserRoomCurrency = args.ApplicationUserRoomsCurrencies.First(s => s.ApplicationUserRoom.Id == connection.ApplicationUserRoom.Id);
                        await _roomHub.Clients.Client(connection.ConnectionId).SendAsync("ReceiveApplicationUserRoomCurrencyRoom", applicationUserRoomCurrency);
                    }
                }
                catch
                {
                    await _dal.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
                    {
                        Id = connection.Id,
                        HasPingBeenSent = connection.HasPingBeenSent,
                        IsOnline = false
                    });
                }
            }

            //var connection = await _services.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

            //if (connection != null)
            //{
            //    if (args.ApplicationUserRoomsCurrencies.Any(s => s.ApplicationUserRoom.Id == connection.ApplicationUserRoom.Id))
            //    {
            //        var applicationUserRoomCurrency = args.ApplicationUserRoomsCurrencies.First(s => s.ApplicationUserRoom.Id == connection.ApplicationUserRoom.Id);
            //        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveApplicationUserRoomCurrency", applicationUserRoomCurrency);
            //    }
            //}
        }

        public async Task RequestServerState(Guid roomId)
        {
            var applicationUserRoomOwnerVM = await _dal.ReadApplicationUserRoomOwnerAsync(roomId);
            var channelOwnerConnections = await _dal.ReadApplicationUserRoomConnectionByApplicationUserRoomIdAsync(applicationUserRoomOwnerVM.Id);

            if (channelOwnerConnections.Count() > 0)
            {
                // ToDo: Do we want this to go to all connections?
                // Only try to grab data from the first connection
                // the ping service will remove stale connections upon rejoining.
                foreach (var connectionInd in channelOwnerConnections)
                {
                    await Clients.Client(connectionInd.ConnectionId).SendAsync("RequestServerState", new ServerStateRequest
                    {
                        ConnectionId = Context.ConnectionId
                    });
                }
            }
        }
        public async Task ReceiveServerState(ServerStateResponse response)
        {
            // this is coming from a room owner, it needs to pass to the new connection
            try
            {

                //if (response.SongQueuedId != Guid.Empty)
                //{
                //    var songQueued = await _services.ReadSongQueuedAsync(response.SongQueuedId);
                //    response.ApplicationUser = songQueued.ApplicationUser;
                //    response.Weight = songQueued.WeightedCurrentValue;
                //}
                //else
                //{
                //    var connection = await _services.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);
                //    var applicationUserRoom = await _services.ReadApplicationUserRoomAsync(connection.ApplicationUserRoom.Id);
                //    response.ApplicationUser = applicationUserRoom.ApplicationUser;
                //    response.Weight = 0;
                //}

                await Clients.Client(response.ConnectionId).SendAsync("RequestPlayFromServer", response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task DequeueNextSong(string roomCode)
        {
            try
            {
                var applicationUserRoomConnection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

                //if (applicationUserRoomConnection == null)
                //{
                //    await CheckConnectionAsync();
                //    await OnConnectedAsync();
                //}

                if (applicationUserRoomConnection.ApplicationUserRoom.IsOwner)
                {
                    var userId = await GetUserIdAsync();
                    var room = await _dal.ReadRoomAsync(roomCode);
                    if (room != null)
                    {
                        var nextSong = await _dal.DequeueSongQueuedAsync(room.Id, userId);

                        if (nextSong != null)
                        {
                            //var song = await _services.ReadSongAsync(nextSong.Song.Id);
                            await Clients.Group(room.RoomCode).SendAsync("RequestPlayFromServer", new PlayFromServerResponse
                            {
                                CurrentTime = 0,
                                SongQueued = nextSong,
                                Weight = nextSong.WeightedValue
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestPlayFromServer(PlayFromServerResponse request)
        {
            var applicationUserRoomConnection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

            if (applicationUserRoomConnection != null && request.SongQueued != null)
            {
                var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(applicationUserRoomConnection.ApplicationUserRoom.Id);
                if (applicationUserRoom != null && applicationUserRoom.IsOwner)
                {
                    await Clients.GroupExcept(applicationUserRoom.Room.RoomCode, new List<string>
                    {
                        Context.ConnectionId
                    }).SendAsync("RequestPlayFromServer", request);
                }
            }

        }
        public async Task RequestPause()
        {
            var applicationUserRoomConnection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);
            if (applicationUserRoomConnection != null)
            {
                var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(applicationUserRoomConnection.ApplicationUserRoom.Id);
                if (applicationUserRoom != null && applicationUserRoom.IsOwner)
                {
                    await Clients.GroupExcept(applicationUserRoom.Room.RoomCode, new List<string>
                    {
                        Context.ConnectionId
                    }).SendAsync("ReceivePause");
                }
            }

        }

        public async Task WagerQuantitySongQueued(WagerQuantitySongQueuedRquest request)
        {
            try
            {
                if (await _dal.WagerQuantitySongQueued(request))
                {
                    var applicationUserRoomConnection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);
                    var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(applicationUserRoomConnection.ApplicationUserRoom.Id);

                    var songsQueued = await _dal.ReadSongQueuedAsync(applicationUserRoom.Room.Id);
                    await Clients.Group(applicationUserRoom.Room.RoomCode).SendAsync("ReceiveSongQueued", songsQueued);

                    var applicationUserRoomCurrency = await _dal.ReadApplicationUserRoomCurrencyRoomAsync(request.ApplicationUserRoomCurrencyRoom.Id);

                    await _roomHub.Clients.Client(Context.ConnectionId).SendAsync("ReceiveApplicationUserRoomCurrencyRoom", applicationUserRoomCurrency);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RequestApplicationUserRoomCurrencies()
        {
            try
            {
                var conection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);
                var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(conection.ApplicationUserRoom.Id);

                var currenciesRoom = await _dal.ReadCurrenciesRoomAsync(applicationUserRoom.Room.Id);
                var applicationUserRoomCurrencies = new List<ApplicationUserRoomCurrencyRoomVM>();

                foreach (var currencyRoom in currenciesRoom)
                {
                    var applicationUserRoomCurrencyRoom = await _dal.ReadApplicationUserRoomCurrencyRoomAsync(applicationUserRoom.Id, currencyRoom.Id);

                    if (applicationUserRoomCurrencyRoom == null)
                    {
                        var roomCurrencies = await _dal.CheckApplicationUserRoomCurrenciesRoomAsync(conection.ApplicationUserRoom.Id);

                        foreach (var roomCurrency in roomCurrencies)
                        {
                            applicationUserRoomCurrencyRoom = await _dal.ReadApplicationUserRoomCurrencyRoomAsync(applicationUserRoom.Id, roomCurrency.Id);
                            applicationUserRoomCurrencies.Add(applicationUserRoomCurrencyRoom);
                        }
                    }
                    applicationUserRoomCurrencies.Add(applicationUserRoomCurrencyRoom);
                }

                await Clients.Caller.SendAsync("ReceiveApplicationUserRoomCurrenciesRoom", applicationUserRoomCurrencies);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SendMessage(ChatMessageVM message)
        {
            if (!message.ApplicationUserRoom.Room.MatureContentChat)
            {
                message.Message = await _service.CleanContent(message.Message);
            }

            if (!string.IsNullOrEmpty(message.Message))
            {
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
        }
        public async Task CreateSongQueued(SongQueuedCreateRequest request)
        {
            try
            {
                var userId = await GetUserIdAsync();
                if (userId != Guid.Empty)
                {
                    var songQueued = await _dal.CreateSongQueuedAsync(request);
                    var applicationUserRoomCurrency = await _dal.ReadApplicationUserRoomCurrencyRoomAsync(request.SongSearchResult.ApplicationUserRoomCurrencyId);

                    if (songQueued != null && applicationUserRoomCurrency != null)
                    {
                        var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(applicationUserRoomCurrency.ApplicationUserRoom.Id);
                        await Clients.Group(applicationUserRoom.Room.RoomCode).SendAsync("ReceiveSongQueued", songQueued);

                        await _roomHub.Clients.Client(Context.ConnectionId).SendAsync("ReceiveApplicationUserRoomCurrencyRoom", applicationUserRoomCurrency);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RequestRoom(string roomCode)
        {
            var userId = await GetUserIdAsync();
            var room = await _dal.ReadRoomAsync(roomCode);

            if (room != null)
            {
                var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(userId, room.Id);

                if (applicationUserRoom == null)
                {
                    applicationUserRoom = await _dal.CreateApplicationUserRoomAsync(new ApplicationUserRoomCreateRequest
                    {
                        IsOnline = true,
                        RoomId = room.Id
                    }, userId);
                }

                var currenciesRooms = await _dal.ReadCurrenciesRoomAsync(room.Id);

                var applicationUserRoomCurrenciesRoom = await _dal.CheckApplicationUserRoomCurrenciesRoomAsync(applicationUserRoom.Id);

                var roomInformation = new RoomInformation
                {
                    ApplicationUserRoom = _mapper.Map<ApplicationUserRoomVM>(applicationUserRoom),
                    ApplicationUserRoomCurrenciesRoom = applicationUserRoomCurrenciesRoom.ToArray()
                };

                await Clients.Caller.SendAsync("ReceiveRoomInformation", roomInformation);

                if (applicationUserRoom.IsOwner)
                {
                    var songNext = await _dal.DequeueSongQueuedAsync(room.Id, userId);
                    await Clients.Client(Context.ConnectionId).SendAsync("RequestPlayFromServer", new PlayFromServerResponse
                    {
                        CurrentTime = 0,
                        PlayerState = (int)ServerStateType.Stopped,
                        SongQueued = songNext,
                        Weight = songNext.WeightedValue
                    });
                }
                else
                {
                    var applicationUserRoomOwnerVM = await _dal.ReadApplicationUserRoomOwnerAsync(room.Id);
                    var channelOwnerConnections = await _dal.ReadApplicationUserRoomConnectionByApplicationUserRoomIdAsync(applicationUserRoomOwnerVM.Id);

                    if (channelOwnerConnections.Count() > 0)
                    {
                        // ToDo: Do we want this to go to all connections?
                        // Only try to grab data from the first connection
                        // the ping service will remove stale connections upon rejoining.
                        foreach (var channelOwnerConnection in channelOwnerConnections)
                        {
                            await Clients.Client(channelOwnerConnection.ConnectionId).SendAsync("RequestServerState", new ServerStateRequest
                            {
                                ConnectionId = Context.ConnectionId
                            });
                        }
                    }
                }
            }
        }
        public async Task RequestSongsQueued(string roomId)
        {
            try
            {
                if (Guid.TryParse(roomId, out var guid))
                {
                    var songsQueued = await _dal.ReadSongsQueuedAsync(guid);
                    await Clients.Caller.SendAsync("ReceiveSongsQueued", songsQueued);
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
                var responses = await _dal.SearchYoutubeLightAsync(searchSnippet);

                await Clients.Caller.SendAsync("ReceiveSearchYoutube", responses);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task PingResponse()
        {
            var connection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

            if (connection != null)
            {
                try
                {
                    await _dal.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
                    {
                        Id = connection.Id,
                        HasPingBeenSent = false,
                        IsOnline = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var context = Context.GetHttpContext();
                var token = context.Request.Query["token"];
                var roomCode = context.Request.Query["roomCode"];

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
                    var applicationUser = await _dal.ReadApplicationUserAsync(userId);

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
                        applicationUser = await _dal.CreateApplicationUserAsync(new ApplicationUserCreateRequest
                        {
                            AspNetUserId = userId,
                            Username = username,
                            RoomCode = roomCodeNew
                        });
                    }

                    // if the room was not specified, then get the default
                    var room = roomCode == "undefined" || roomCode == ""
                        ? await _dal.ReadRoomAsync(applicationUser.Room.Id)
                        : await _dal.ReadRoomAsync(roomCode);

                    if (room != null)
                    {
                        var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(applicationUser.Id, room.Id);

                        if (applicationUserRoom == null)
                        {
                            applicationUserRoom = await _dal.CreateApplicationUserRoomAsync(new ApplicationUserRoomCreateRequest
                            {
                                IsOnline = true,
                                RoomId = room.Id
                            }, applicationUser.Id);
                        }

                        var connection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

                        connection = connection == null
                            ? await _dal.CreateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionCreateRequest
                            {
                                ApplicationUserRoomId = applicationUserRoom.Id,
                                ConnectionId = Context.ConnectionId,
                                IsOnline = true,
                                ConnectionType = ConnectionType.RoomHub
                            })
                            : await _dal.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
                            {
                                HasPingBeenSent = connection.HasPingBeenSent,
                                IsOnline = true,
                                Id = connection.Id,
                                ApplicationUserRoomId = applicationUserRoom.Id
                            });

                        if (applicationUserRoom.IsOwner)
                        {
                            var applicationUserRoomConnections = await _dal.ReadApplicationUserRoomConnectionByApplicationUserRoomIdAsync(applicationUserRoom.Id);

                            foreach (var appConnection in applicationUserRoomConnections)
                            {
                                await Clients.Client(appConnection.ConnectionId).SendAsync("ForceServerDisconnect");

                                var applicationUserRoomTemp = await _dal.ReadApplicationUserRoomAsync(appConnection.ApplicationUserRoom.Id);

                                await Clients.Group(applicationUserRoomTemp.Room.RoomCode).SendAsync("ReceiveApplicationUserRoomOffline", applicationUserRoomTemp);

                                await _dal.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
                                {
                                    ApplicationUserRoomId = appConnection.ApplicationUserRoom.Id,
                                    HasPingBeenSent = appConnection.HasPingBeenSent,
                                    Id = appConnection.Id,
                                    IsOnline = false
                                });
                            }
                        }

                        var applicationUserRoomCurrenciesRoom = await _dal.CheckApplicationUserRoomCurrenciesRoomAsync(applicationUserRoom.Id);

                        await base.OnConnectedAsync();

                        await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomCode);
                        await Clients.Group(applicationUserRoom.Room.RoomCode).SendAsync("ReceiveApplicationUserRoomOnline", applicationUserRoom);

                        var roomEntity = await _dal.ReadRoomAsync(applicationUserRoom.Room.Id);

                        var roomInformation = new RoomInformation
                        {
                            Room = _mapper.Map<RoomDTO>(room),
                            ApplicationUserRoom = _mapper.Map<ApplicationUserRoomVM>(applicationUserRoom),
                            ApplicationUserRoomCurrenciesRoom = applicationUserRoomCurrenciesRoom.ToArray(),
                            RoomOwner = roomEntity.ApplicationUser
                        };

                        await Clients.Caller.SendAsync("ReceiveRoomInformation", roomInformation);

                        if (applicationUserRoom.IsOwner)
                        {
                            var songNext = await _dal.DequeueSongQueuedAsync(room.Id, applicationUserRoom.ApplicationUser.Id);
                            if (songNext != null)
                            {
                                await Clients.Client(Context.ConnectionId).SendAsync("RequestPlayFromServer", new PlayFromServerResponse
                                {
                                    CurrentTime = 0,
                                    PlayerState = (int)ServerStateType.Stopped,
                                    SongQueued = songNext,
                                    Weight = songNext.WeightedValue
                                });
                            }
                        }
                        else
                        {
                            var applicationUserRoomOwnerVM = await _dal.ReadApplicationUserRoomOwnerAsync(room.Id);
                            var channelOwnerConnections = await _dal.ReadApplicationUserRoomConnectionByApplicationUserRoomIdAsync(applicationUserRoomOwnerVM.Id);

                            if (channelOwnerConnections.Count() > 0)
                            {
                                // ToDo: Do we want this to go to all connections?
                                // Only try to grab data from the first connection
                                // the ping service will remove stale connections upon rejoining.
                                foreach (var channelOwnerConnection in channelOwnerConnections)
                                {
                                    await Clients.Client(channelOwnerConnection.ConnectionId).SendAsync("RequestServerState", new ServerStateRequest
                                    {
                                        ConnectionId = Context.ConnectionId
                                    });
                                }
                            }
                        }
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
            try
            {
                await base.OnDisconnectedAsync(exception);
                var connection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);
                if (connection != null)
                {
                    connection = await _dal.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
                    {
                        Id = connection.Id,
                        HasPingBeenSent = connection.HasPingBeenSent,
                        IsOnline = false
                    });

                    var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(connection.ApplicationUserRoom.Id);

                    if (await _dal.UpdateApplicationUserRoomAndRoomToOfflineAsync(applicationUserRoom.Id))
                    {
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, applicationUserRoom.Room.RoomCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected virtual async Task<Guid> GetUserIdAsync()
        {
            var applicationUserRoomConnection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

            if (applicationUserRoomConnection == null)
            {
                applicationUserRoomConnection = await CheckConnectionAsync();

                if (applicationUserRoomConnection == null)
                {
                    await _roomHub.Clients.Client(Context.ConnectionId).SendAsync("ForceServerDisconnect");
                    return Guid.Empty;
                }
            }

            var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(applicationUserRoomConnection.ApplicationUserRoom.Id);
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
                var applicationUser = await _dal.ReadApplicationUserAsync(userId);

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
                    applicationUser = await _dal.CreateApplicationUserAsync(new ApplicationUserCreateRequest
                    {
                        AspNetUserId = userId,
                        Username = username,
                        RoomCode = roomCodeNew
                    });
                }

                // if the room was not specified, then get the default
                var room = await _dal.ReadRoomAsync(applicationUser.Room.Id);

                if (room != null)
                {
                    var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(applicationUser.Id, room.Id);

                    if (applicationUserRoom == null)
                    {
                        applicationUserRoom = await _dal.CreateApplicationUserRoomAsync(new ApplicationUserRoomCreateRequest
                        {
                            IsOnline = true,
                            RoomId = room.Id
                        }, applicationUser.Id);
                    }

                    var connection = await _dal.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);

                    connection = connection == null
                    ? await _dal.CreateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionCreateRequest
                    {
                        ApplicationUserRoomId = applicationUserRoom.Id,
                        ConnectionId = Context.ConnectionId,
                        IsOnline = true
                    })
                    : await _dal.UpdateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionUpdateRequest
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

            if (_currencyPoll != null)
            {
                _currencyPoll.PollingEvent -= async (s, e) => await OnCurrencyPollEvent(s, e);
            }

            base.Dispose(disposing);
        }
    }
}
