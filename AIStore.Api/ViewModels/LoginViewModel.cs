using System.ComponentModel.DataAnnotations;

namespace AIStore.Api.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required]
        [StringLength(18, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
