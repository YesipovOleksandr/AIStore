using AIStore.Domain.Enums;

namespace AIStore.Domain.Models.Users
{
    public class User
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsEmailСonfirm { get; set; } = false;
        public List<UserRoles> UserRoles { get; set; }
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}