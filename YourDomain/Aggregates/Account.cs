using Infrastructure;
using YourDomain.Events;

namespace YourDomain.Aggregates
{
    public class Account : Aggregate
    {
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Password { get; private set; }
        public bool IsActive { get; private set; }

        public Account(string email, string firstName, string lastName, string password)
        {
            AddEvent(new AccountCreated(email, firstName, lastName));

            Password = password;
            LastName = lastName;
            FirstName = firstName;
            Email = email;
            IsActive = true;
        }

        private Account(){}//Mandotary for RavenDb persistance
    }
}
