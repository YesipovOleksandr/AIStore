using System.ComponentModel.DataAnnotations;

namespace AIStore.Api.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Login { get; set; }
    }
}
