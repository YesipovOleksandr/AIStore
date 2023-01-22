using AIStore.Domain.Models.RecoverPassword;
using AIStore.Domain.Models.Verify;

namespace AIStore.Domain.Abstract.Repository
{
    public interface IRecoverPasswordRepository
    {
        Task<RecoverPasswordCode> Create(RecoverPasswordCode item);
        Task<bool> ExistUser(long userId);
        RecoverPasswordCode GetByUserId(long Id);
        void Update(RecoverPasswordCode item);
        void Save();
        void Remove(RecoverPasswordCode item);
    }
}
