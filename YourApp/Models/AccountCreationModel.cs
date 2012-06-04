using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace YourApp.Models
{
    public class AccountCreationModel
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [StringLength(int.MaxValue, MinimumLength = 5)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirmation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}