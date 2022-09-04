namespace AIStore.Api.ViewModels
{
    public class AuthViewModel
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
