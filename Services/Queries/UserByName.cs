using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using YourDomain.Aggregates;
using Raven.Client.Linq.Indexing;

namespace Services.Queries
{
    public class UserByName : AbstractIndexCreationTask<Account,User>
    {
        public UserByName()
        {
            Map = users => from user in users
                           select new {Name = string.Format("{0} {1}",user.FirstName,user.LastName).Trim()};

            Store(user=>user.Name,FieldStorage.Yes);
            Indexes.Add(u=> u.Name,FieldIndexing.Analyzed);

        }
    }
}