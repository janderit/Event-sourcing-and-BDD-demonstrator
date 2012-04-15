using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Base
{
    public abstract class EventSourcedAggregate
    {

        private readonly List<Event> _uncommitted = new List<Event>();
        private bool _disabled;

        public IEnumerable<Event> UncommittedEvents { get { return _uncommitted.ToList(); } }

        internal void Commit()
        {
            _uncommitted.Clear();
        }

        internal void Disable()
        {
            _disabled = true;
        }

        internal void LoadFromHistory(IEnumerable<Event> history)
        {
            history.ToList().ForEach(Dispatch);
        }

        internal void Publish(Event e)
        {
            if (_disabled) throw new InvalidOperationException("Aggregat ist ungültig.");

            _uncommitted.Add(e);
            Dispatch(e);
        }

        protected abstract void Dispatch(Event e);
    }
}
