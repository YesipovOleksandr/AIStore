using System.ComponentModel.DataAnnotations;

namespace AIStore.DAL.Entities
{
    public class RecoverPasswordCode : Entity<long>
    {
        [Required]
        public long UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime ExpirationTime { get; set; }
    }
}


