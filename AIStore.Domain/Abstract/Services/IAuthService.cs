using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Services
{
    public interface IAuthService
    {
        Task<User> Registration(User user);
        Task<User> Authenticate(User user,bool isPassword=true);
        Task<bool> IsUserLoginExist(string login);
        Task EmailConfirmation(User user,string code);
        Task SendActivationEmail(User user);
        Task SendForgotPassword(User model);
        Task ResetPassword(User model,string newPassword);
    }
}
