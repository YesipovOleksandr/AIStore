using AIStore.Domain.Enums;

namespace AIStore.DAL.Entities
{
    public class User : Entity<long>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsEmailСonfirm { get; set; }
        public List<UserRoles> UserRoles { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
