using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.Dto
{
    public class ChangePasswordDto
    {
        [MaxLength(32), MinLength(6)]
        public string OldPassword { get; set; }

        [MaxLength(32), MinLength(6)]
        public string NewPassword { get; set; }

        [MaxLength(32), MinLength(6)]
        public string ConfirmNewPassword { get; set; }
    }
}
