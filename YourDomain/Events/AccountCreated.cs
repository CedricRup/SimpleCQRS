using Infrastructure.Events;

namespace YourDomain.Events
{
    public class AccountCreated : IEvent
    {
        public readonly string Email;
        public readonly string FirstName;
        public readonly string LastName;

        public AccountCreated(string email, string firstName, string lastName)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}