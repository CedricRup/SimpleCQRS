using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;
using YourDomain.Aggregates;

namespace Services.Queries
{
    public class UserQueries
    {
        private readonly IDocumentStore documentStore;

        public UserQueries(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public IEnumerable<User> Search(string name)
        {
            using (var session = documentStore.OpenSession())
            {
                return session.Query<User, UserByName>().Where(u => u.Name.StartsWith(name)).ToList();
            }
        }
    }
}
