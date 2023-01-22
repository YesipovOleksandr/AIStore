using AIStore.Domain.Models.Verify;

namespace AIStore.Domain.Abstract.Services.Verifier
{
    public interface IVerifierService
    {
        Task<VerifyCode> SetVerificationCode(long userId);
        void VerificationCode(long userId, string code);
    }
}
