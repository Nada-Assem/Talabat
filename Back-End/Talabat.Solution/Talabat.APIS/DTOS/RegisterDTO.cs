using System.ComponentModel.DataAnnotations;

namespace Talabat.APIS.DTOS
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$",
        ErrorMessage = "The password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]

        public string Password { get; set; }
        [Required]

        public string PhoneNumber { get; set; }
        [Required]

        public string DisplayName {  get; set; }
    }
}
