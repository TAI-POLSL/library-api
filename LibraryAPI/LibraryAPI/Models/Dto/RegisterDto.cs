using LibraryAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.Dto
{
    public class RegisterDto
    {
        public string Username { get; set; }

        [MaxLength(32), MinLength(6)]
        public string Password { get; set; }

        [MaxLength(32), MinLength(6)]
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public UserRoles Roles { get; set; }

        [MaxLength(100), EmailAddress]
        public string Email { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
