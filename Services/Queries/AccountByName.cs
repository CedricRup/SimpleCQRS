using System.Linq;
using Raven.Client.Indexes;
using YourDomain.Aggregates;

namespace Services.Queries
{
    public class AccountByName : AbstractIndexCreationTask<Account>
    {
        public AccountByName()
        {
            Map = accounts => from account in accounts
                           select new {account.FirstName,account.LastName};

        }
    }
}