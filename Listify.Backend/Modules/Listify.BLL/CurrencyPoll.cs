using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.BLL
{
    //public class CurrencyPoll : BasePoll<CurrencyPollEventArgs>
    //{
    //    public CurrencyPoll(ListifyServices service) : base(service)
    //    {

    //    }

    //    protected override async Task TimerTickEvents()
    //    {
    //        var rooms = await _services.ReadRoomsAsync();

    //        foreach (var room in rooms)
    //        {
    //            try
    //            {
    //                var roomVm = await _services.ReadRoomAsync(room.Id);
    //                if (roomVm.IsRoomOnline)
    //                {
    //                    foreach (var currency in roomVm.Currencies)
    //                    {
    //                        var currencyVM = await _services.ReadCurrencyAsync(currency.Id);
    //                        if (currency != null &&
    //                            currencyVM.TimestampLastUpdate + TimeSpan.FromSeconds(currencyVM.TimeSecBetweenTick) < DateTime.UtcNow)
    //                        {
    //                            var applicationUserRoomsCurrencies = await _services.AddCurrencyQuantityToAllUsersInRoomAsync(room);

    //                            FirePollingEvent(this, new CurrencyPollEventArgs
    //                            {
    //                                PollingEventType = PollingEventType.CurrencyPoll,
    //                                ApplicationUserRoomsCurrencies = applicationUserRoomsCurrencies
    //                            });
    //                        }
    //                    }
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //            }
    //        }
    //    }
    //}
}
