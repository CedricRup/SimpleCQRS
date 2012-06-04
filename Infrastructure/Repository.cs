using System;
using Infrastructure.Events;
using Raven.Client;

namespace Infrastructure
{
    public interface IRepository<T> where T : Aggregate
    {
        void Save(T entity);
        T Load(Guid id);
    }

    public class Repository<T> : IRepository<T> where T : Aggregate
    {
        //RavenDB pour la persistence
        private readonly IDocumentStore documentStore;
        //On a besoin d'une evnt publisher pour acheminer les évenements metier
        private readonly IEventPublisher eventPublisher;

        public Repository(IDocumentStore documentStore, IEventPublisher eventPublisher)
        {
            this.documentStore = documentStore;
            this.eventPublisher = eventPublisher;
        }

        public void Save(T entity)
        {
            using (var session = documentStore.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
            }
            foreach (var uncomittedEvent in entity.GetUncomittedEvents())
            {
                eventPublisher.Publish(uncomittedEvent);
            }
            entity.CommitEvents();
        }

        public T Load(Guid id)
        {
            using (var session = documentStore.OpenSession())
            {
                return session.Load<T>(id);
            }
        }
    }
}
