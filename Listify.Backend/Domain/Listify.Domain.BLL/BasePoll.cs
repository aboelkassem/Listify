using Listify.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Listify.Domain.BLL
{
    public abstract class BasePoll<T> : IDisposable where T : BasePollingEventArgs
    {
        protected Timer _timer;

        protected readonly IListifyServices _service;
        protected volatile bool _isTimerEventsRunning;

        private event ListifyEventHandler<T> _pollingEvent;

        public BasePoll(IListifyServices service)
        {
            _service = service;
        }

        public virtual void Start(int pollingIntervalMS)
        {
            _timer = new Timer(OnTimerTick, null, pollingIntervalMS, pollingIntervalMS);
        }
        public virtual void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
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
                        await TimerTickEvent();
                    }
                    catch
                    {
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

        public void Dispose()
        {
            Stop();
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
