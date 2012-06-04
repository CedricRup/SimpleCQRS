using Infrastructure.Events;

namespace YourDomain.Commands
{
    public class CreateAccount : ICommand
    {
        public readonly string Email;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly string Password;

        public CreateAccount(string email, string firstName, string lastName, string password)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }
    }
}