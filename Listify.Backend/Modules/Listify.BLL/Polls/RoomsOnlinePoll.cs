using Listify.BLL.Events.Args;
using Listify.BLL.Polls;
using Listify.DAL;
using Listify.Domain.BLL;
using Listify.Domain.Lib.Enums;
using Listify.Lib.Requests;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.BLL
{
    public class RoomsOnlinePoll : BasePoll<RoomsOnlinePollEventArgs>, IRoomsOnlinePoll
    {
        public RoomsOnlinePoll(IListifyDAL dal): base(dal)
        {

        }

        protected override async Task TimerTickEvents()
        {
            try
            {
                var rooms = await _dal.ReadRoomsAsync();

                foreach (var room in rooms)
                {
                    var owner = await _dal.ReadApplicationUserRoomOwnerAsync(room.Id);

                    if (owner != null)
                    {
                        //var applicationUserRoom = await _dal.ReadApplicationUserRoomAsync(owner.ApplicationUser.Id, room.Id);

                        var ownerConnections = await _dal.ReadApplicationUserRoomConnectionByApplicationUserRoomIdAsync(owner.Id);

                        if (room.IsRoomOnline)
                        {
                            try
                            {
                                if (!ownerConnections.Any(s => s.ConnectionType == ConnectionType.RoomHub && s.IsOnline))
                                {
                                    await _dal.UpdateRoomAsync(new RoomUpdateRequest
                                    {
                                        IsRoomOnline = false,
                                        Id = room.Id,
                                        IsRoomPlaying = false,
                                        RoomGenres = room.RoomGenres.ToArray()
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                if (ownerConnections.Any(s => s.ConnectionType == ConnectionType.RoomHub && s.IsOnline))
                                {
                                    await _dal.UpdateRoomAsync(new RoomUpdateRequest
                                    {
                                        IsRoomOnline = true,
                                        Id = room.Id,
                                        IsRoomPlaying = room.IsRoomPlaying,
                                        RoomGenres = room.RoomGenres.ToArray()
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
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
