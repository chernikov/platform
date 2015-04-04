using System.ComponentModel.DataAnnotations;

namespace platformAthletic.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}