using System;
using System.Collections.Generic;
using Infrastructure.Events;
using Newtonsoft.Json;

namespace Infrastructure
{
    public abstract class Aggregate
    {
        public Guid Id { get; protected set; }
        private readonly List<IEvent> events = new List<IEvent>(); 

        public IEnumerable<IEvent> GetUncomittedEvents()
        {
            return events.AsReadOnly();
        }

        protected void AddEvent(IEvent @event)
        {
            events.Add(@event);
        }

        public void CommitEvents()
        {
            events.Clear();
        }
    }
}
