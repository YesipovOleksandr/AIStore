using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Services
{
    public interface IAuthService
    {
        User Registration(User user);
        User Authenticate(User user,bool isPassword=true);
        bool IsUserLoginExist(string login);
        void EmailConfirmation(User user,string code);
    }
}
