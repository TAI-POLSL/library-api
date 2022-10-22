using System.ComponentModel.DataAnnotations;
using LibraryAPI.Enums;

namespace LibraryAPI.Models.Dto
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Address => $"{StreetAddress} {PostalCode} {City}";
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
