using Infrastructure;

namespace YourDomain.Aggregates
{
    public class Account 
    {

        public string Id { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Password { get; private set; }
        public bool IsActive { get; private set; }

        public Account(string email, string firstName, string lastName, string password)
        {

            Password = password;
            LastName = lastName;
            FirstName = firstName;
            Email = email;
            IsActive = true;
        }

        private Account(){}//Mandotary for RavenDb persistance
    }
}
