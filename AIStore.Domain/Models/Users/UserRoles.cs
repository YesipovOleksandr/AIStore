using AIStore.Domain.Enums;

namespace AIStore.Domain.Models.Users
{
    public class UserRoles
    {
        public Role Role { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
