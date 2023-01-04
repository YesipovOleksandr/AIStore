using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Abstract.Services.Verifier;
using AIStore.Domain.Models.Users;
using AIStore.Domain.Models.Verify;

namespace AIStore.BLL.Services.Verifier
{
    public class VerifierService : IVerifierService
    {
        private const int TIME_OUT_MINUTES = 10;
        private readonly IVerifyRepository _verifyRepository;

        public VerifierService(IVerifyRepository verifyRepository)
        {
            _verifyRepository = verifyRepository;
        }

        public VerifyCode SetVerificationCode(long userId)
        {
            var ExistModel = new VerifyCode();

            if (!_verifyRepository.ExistUser(userId))
            {
                ExistModel.UserId = userId;
                ExistModel.ExpirationTime = DateTime.UtcNow.AddMinutes(TIME_OUT_MINUTES);
                ExistModel.Code = GetNewVerifyCode();

                _verifyRepository.Create(ExistModel);
            }
            else
            {
                ExistModel = _verifyRepository.GetByUserId(userId);
                ExistModel.ExpirationTime = DateTime.UtcNow.AddMinutes(TIME_OUT_MINUTES);
                ExistModel.Code = GetNewVerifyCode();
                _verifyRepository.Update(ExistModel);
            }
            return ExistModel;
        }

        public void VerificationCode(long userId, string code)
        {
            var verify = _verifyRepository.GetByUserId(userId);
            if (verify == null)
            {
                throw new Exception("verify is null");
            }
            if (verify.ExpirationTime <= DateTime.UtcNow)
            {
                throw new Exception("time is over");
            }
            else if (verify.Code != code)
            {
                throw new Exception("wrong code");
            }
            _verifyRepository.Remove(verify);
        }

        private string GetNewVerifyCode()
        {
            Random rand = new Random();
            int num = rand.Next(1111, 9999);
            return num.ToString();
        }
    }
}
