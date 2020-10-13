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
        public CurrencyPoll(IListifyDAL dal) : base(dal)
        {

        }

        protected override async Task TimerTickEvents()
        {
            try
            {
                var rooms = await _dal.ReadRoomsAsync();

                foreach (var room in rooms)
                {
                    var currenciesRoom = await _dal.ReadCurrenciesRoomAsync(room.Id);
                    
                    foreach (var currencyRoom in currenciesRoom)
                    {
                        //var roomVM = await _dal.ReadRoomAsync(room.Id);

                        if (currencyRoom.TimestampLastUpdate + TimeSpan.FromSeconds(currencyRoom.Currency.TimeSecBetweenTick) < DateTime.UtcNow)
                        {
                            var applicationUserRoomsCurrencies = await _dal.AddCurrencyQuantityToAllUsersInRoomAsync(room.Id, currencyRoom.Id, currencyRoom.Currency.QuantityIncreasePerTick, TransactionType.PollingCurrency);

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
