using AIStore.Domain.Models.Users;
using AIStore.Domain.Models.Verify;

namespace AIStore.Domain.Abstract.Services.Verifier
{
    public interface IVerifierService
    {
        VerifyCode SetVerificationCode(User model);
        void VerificationCode(long userId, string code);
    }
}
