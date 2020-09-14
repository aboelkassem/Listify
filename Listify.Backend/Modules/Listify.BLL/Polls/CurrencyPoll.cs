using Listify.BLL.Events.Args;
using Listify.DAL;
using Listify.Domain.BLL;
using Listify.Domain.Lib.Enums;
using System;
using System.Threading.Tasks;

namespace Listify.BLL.Polls
{
    public class CurrencyPoll : BasePoll<CurrencyPollEventArgs>, ICurrencyPoll
    {
        public CurrencyPoll(IListifyServices service) : base(service)
        {

        }

        protected override async Task TimerTickEvents()
        {
            try
            {
                var rooms = await _service.ReadRoomsAsync();

                foreach (var room in rooms)
                {
                    var currenciesRoom = await _service.ReadCurrenciesRoomAsync(room.Id);
                    
                    foreach (var currencyRoom in currenciesRoom)
                    {

                        var roomVM = await _service.ReadRoomAsync(room.Id);

                        //var currencyVM = await _service.ReadCurrencyAsync(currency.Id);
                        if (currencyRoom.TimestampLastUpdate + TimeSpan.FromSeconds(currencyRoom.Currency.TimeSecBetweenTick) < DateTime.UtcNow)
                        {
                            var applicationUserRoomsCurrencies = await _service.AddCurrencyQuantityToAllUsersInRoomAsync(room.Id, currencyRoom.Id, currencyRoom.Currency.QuantityIncreasePerTick, TransactionType.PollingCurrency);

                            FirePollingEvent(this, new CurrencyPollEventArgs
                            {
                                PollingEventType = PollingEventType.CurrencyPoll,
                                ApplicationUserRoomsCurrencies = applicationUserRoomsCurrencies,
                                Room = room
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
    }
}
