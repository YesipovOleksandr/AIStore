using AIStore.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStore.DAL.Entities
{
    public class UserRoles
    {
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public Role Role { get; set; }

        [Required]
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
