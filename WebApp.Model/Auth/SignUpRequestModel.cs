using System.ComponentModel.DataAnnotations;

namespace WebApp.Model.Auth
{
    public class SignUpRequestModel
    {
        [Required(ErrorMessage ="Email is required.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]

        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]

        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public long Phone { get; set; } 
    }
}
