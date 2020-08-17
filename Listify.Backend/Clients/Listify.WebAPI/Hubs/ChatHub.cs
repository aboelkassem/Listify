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

namespace Listify.WebAPI.Hubs
{
    public class ChatHub : Hub, IDisposable
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IHubContext<ChatHub> _chatHub;
        protected readonly IListifyServices _services;
        protected readonly IMapper _mapper;

        public ChatHub(
            ApplicationDbContext context,
            IHubContext<ChatHub> chatHub,
            IListifyServices services,
            IMapper mapper)
        {
            _context = context;
            _chatHub = chatHub;
            _services = services;
            _mapper = mapper;
        }

        public async Task SendMessage(ChatMessageVM message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task RequestUserInformation()
        {
            try
            {
                var applicationUserRoomConnection = await _services.ReadApplicationUserRoomConnectionAsync(Context.ConnectionId);
                var applicationUserRoom = await _services.ReadApplicationUserRoomAsync(applicationUserRoomConnection.ApplicationUserRoom.Id);
                var applicationUser = await _services.ReadApplicationUserAsync(applicationUserRoom.ApplicationUser.Id);

                await Clients.Caller.SendAsync("ReceiveUserInformation", applicationUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var context = Context.GetHttpContext();
                var token = context.Request.Query["token"];
                var roomCode = context.Request.Query["roomCode"];

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

                    // the room is attached here
                    applicationUser = await _services.CreateApplicationUserAsync(new ApplicationUserCreateRequest
                    {
                        AspNetUserId = userId,
                        Username = username,
                        RoomCode = roomCodeNew
                    });
                }

                // if the room was not specified, then get the default
                var room = roomCode == "undefined" || roomCode == ""
                    ? await _services.ReadRoomAsync(applicationUser.Room.Id)
                    : await _services.ReadRoomAsync(roomCode);

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

                    var connection = await _services.CreateApplicationUserRoomConnectionAsync(new ApplicationUserRoomConnectionCreateRequest
                    {
                        ApplicationUserRoomId = applicationUserRoom.Id,
                        ConnectionId = Context.ConnectionId,
                        IsOnline = true
                    });

                    await base.OnConnectedAsync();

                    var applicationUserRoomVM = _mapper.Map<ApplicationUserRoomVM>(applicationUserRoom);
                    applicationUserRoomVM.ApplicationUser = _mapper.Map<ApplicationUserDTO>(applicationUser);
                    applicationUserRoomVM.Room = _mapper.Map<RoomDTO>(room);

                    await Clients.Caller.SendAsync("ReceiveData", new ChatData
                    {
                        ApplicationUserRoom = applicationUserRoomVM
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
