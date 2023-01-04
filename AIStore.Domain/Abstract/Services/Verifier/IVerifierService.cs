using AIStore.Domain.Models.Users;
using AIStore.Domain.Models.Verify;

namespace AIStore.Domain.Abstract.Services.Verifier
{
    public interface IVerifierService
    {
        VerifyCode SetVerificationCode(long userId);
        void VerificationCode(long userId, string code);
    }
}
