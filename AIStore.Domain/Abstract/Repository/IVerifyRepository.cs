using AIStore.Domain.Models.Users;
using AIStore.Domain.Models.Verify;

namespace AIStore.Domain.Abstract.Repository
{
    public interface IVerifyRepository
    {
        VerifyCode GetById(long Id);
        VerifyCode GetByUserId(long Id);
        VerifyCode Create(VerifyCode item);
        bool ExistUser(long userId);
        void Update(VerifyCode item);
        void Save();
        void Remove(VerifyCode item);
    }
}
