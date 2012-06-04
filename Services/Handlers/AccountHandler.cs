using Infrastructure;
using Infrastructure.Events;
using YourDomain.Aggregates;
using YourDomain.Commands;

namespace Services.Handlers
{
    public class AccountHandler : IHandle<CreateAccount>
    {
        private readonly IRepository<Account> accountRepository;

        public AccountHandler(IRepository<Account> accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Handle(CreateAccount message)
        {
            var account = new Account(message.Email, message.FirstName, message.LastName, message.Password);
            accountRepository.Save(account);
        }
    }
}
