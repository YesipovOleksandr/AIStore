namespace AIStore.Domain.Models.Email
{
    public class EmailConfirm
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
        public string ViewName { get; set; }
    }
}
