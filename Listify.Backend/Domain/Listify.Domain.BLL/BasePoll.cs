using Listify.DAL;
using Listify.Domain.BLL.Events;
using Listify.Domain.BLL.Events.Args;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Listify.Domain.BLL
{
    public abstract class BasePoll<T> : IDisposable, IBasePoll<T> where T : BasePollingEventArgs
    {
        protected Timer _timer;

        protected readonly IListifyDAL _dal;
        protected volatile bool _isTimerEventsRunning;

        private event ListifyEventHandler<T> _pollingEvent;

        public BasePoll(IListifyDAL dal)
        {
            _dal = dal;
        }

        public virtual void Start(int pollingIntervalMS)
        {
            _timer = new Timer(OnTimerTick, null, pollingIntervalMS, pollingIntervalMS);
        }
        public virtual async Task StopAync()
        {
            if (_timer != null)
            {
                await _timer.DisposeAsync();
                _timer = null;
                _isTimerEventsRunning = false;
            }
        }

        protected virtual void OnTimerTick(object state)
        {
            if (!_isTimerEventsRunning)
            {
                _isTimerEventsRunning = true;
                Task.Run(async () =>
                {
                    try
                    {
                        await TimerTickEvents();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    _isTimerEventsRunning = false;
                });
            }
        }

        protected abstract Task TimerTickEvents();
        protected virtual void FirePollingEvent(object sender, T e)
        {
            _pollingEvent?.Invoke(sender, e);
        }

        public async void Dispose()
        {
            await StopAync();
        }

        public event ListifyEventHandler<T> PollingEvent
        {
            add
            {
                _pollingEvent += value;
            }
            remove
            {
                _pollingEvent -= value;
            }
        }
    }
}
