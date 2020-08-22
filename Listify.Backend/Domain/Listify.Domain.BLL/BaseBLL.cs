using Listify.DAL;
using Listify.Domain.BLL.Args;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Listify.Domain.BLL
{
    public abstract class BaseBLL<T> : IDisposable where T : BasePollingEventArgs
    {
        protected Timer _timer;

        protected readonly IListifyServices _service;
        protected volatile bool _isTimerEventsRunning;

        private event ListifyEventHnadler<T> _pollingEvent;

        public BaseBLL(IListifyServices service)
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

            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
