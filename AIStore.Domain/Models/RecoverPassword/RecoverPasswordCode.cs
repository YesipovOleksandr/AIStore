namespace AIStore.Domain.Models.RecoverPassword
{
    public class RecoverPasswordCode
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
