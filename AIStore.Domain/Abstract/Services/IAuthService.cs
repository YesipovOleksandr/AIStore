using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Services
{
    public interface IAuthService
    {
        Task<User> Registration(User user);
        User Authenticate(User user,bool isPassword=true);
        bool IsUserLoginExist(string login);
        void EmailConfirmation(User user,string code);
        Task SendActivationEmail(User user);
        Task SendForgotPassword(User model);
        void ResetPassword(User model,string newPassword);
    }
}
