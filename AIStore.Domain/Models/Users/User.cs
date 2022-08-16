using AIStore.Domain.Enums;

namespace AIStore.Domain.Models.Users
{
    public class User
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<UserRoles> UserRoles { get; set; }
    }
}