using System.Linq;
using Raven.Client.Indexes;
using YourDomain.Aggregates;

namespace Services.Queries
{
    public class UserByName : AbstractIndexCreationTask<User>
    {
        public UserByName()
        {
            Map = users => from user in users
                           select new {user.Name};

            //Indexes.Add(u=> u.Name,FieldIndexing.Analyzed);
        }
    }
}