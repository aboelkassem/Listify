using Listify.BLL.Args;
using Listify.DAL;
using Listify.Domain.BLL;
using Listify.Domain.Lib.Enums;
using System;
using System.Threading.Tasks;

namespace Listify.BLL
{
    public class CurrencyPoll : BasePoll<CurrencyPollEventArgs>
    {
        public CurrencyPoll(IListifyServices service) : base(service)
        {

        }

        protected override async Task TimerTickEvents()
        {
            var rooms = await _service.ReadRoomsAsync();
            var currencies = await _service.ReadCurrenciesAsync();
            //RE: ToDo: Fix this with correct auth and routing
            var currencyActive = currencies[0];
            var currencyVM = await _service.ReadCurrencyAsync(currencyActive.Id);

            if (currencyVM != null)
            {
                foreach (var room in rooms)
                {
                    if (room.IsRoomOnline)
                    {
                        try
                        {
                            var roomVM = await _service.ReadRoomAsync(room.Id);

                            foreach (var currency in currencies)
                            {
                                //var currencyVM = await _service.ReadCurrencyAsync(currency.Id);
                                if (currency != null &&
                                    currencyVM.TimestampLastUpdated + TimeSpan.FromSeconds(currencyVM.TimeSecBetweenTick) < DateTime.UtcNow)
                                {
                                    var applicationUserRoomsCurrencies = await _service.AddCurrencyQuantityToAllUsersInRoomAsync(room.Id, currency.Id, currency.QuantityIncreasePerTick, TransactionType.PollingCurrency);

                                    FirePollingEvent(this, new CurrencyPollEventArgs
                                    {
                                        PollingEventType = PollingEventType.CurrencyPoll,
                                        ApplicationUserRoomsCurrencies = applicationUserRoomsCurrencies
                                    });
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
        }
    }
}
