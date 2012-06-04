using Infrastructure.Events;
using Raven.Client;
using YourDomain.Aggregates;
using YourDomain.Events;

namespace Services.Handlers
{
    public class UserHandler : IHandle<AccountCreated>
    {
        private readonly IDocumentStore documentStore;

        public UserHandler(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public void Handle(AccountCreated message)
        {
            using (var session = documentStore.OpenSession())
            {
                var user = new User {Name = string.Format("{0} {1}", message.FirstName, message.LastName).Trim()};
                session.Store(user);
                session.SaveChanges();
            }
        }
    }
}

    