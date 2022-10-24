using LibraryAPI.Enums;

namespace LibraryAPI.Models
{
    public class CookieUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }    
        public UserRoles Role { get; set; }

        public string getUserInfo => $"{Username}/{Role}/{Id}";
    }
}
