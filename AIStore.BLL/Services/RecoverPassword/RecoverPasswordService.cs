using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Abstract.Services.RecoverPassword;
using AIStore.Domain.Models.RecoverPassword;

namespace AIStore.BLL.Services.RecoverPassword
{
    public class RecoverPasswordService: IRecoverPasswordService
    {
        private const int TIME_OUT_MINUTES = 10;
        private readonly IRecoverPasswordRepository _recoverPasswordRepository;

        public RecoverPasswordService(IRecoverPasswordRepository recoverPasswordRepositor)
        {
            _recoverPasswordRepository = recoverPasswordRepositor;
        }

        public async Task<RecoverPasswordCode> SetRecoverPasswordCode(long userId)
        {
            var ExistModel = new RecoverPasswordCode();

            if (!await _recoverPasswordRepository.ExistUser(userId))
            {
                ExistModel.UserId = userId;
                ExistModel.ExpirationTime = DateTime.UtcNow.AddMinutes(TIME_OUT_MINUTES);
                ExistModel.Code = GetNewVerifyCode();

                await _recoverPasswordRepository.Create(ExistModel);
            }
            else
            {
                ExistModel = _recoverPasswordRepository.GetByUserId(userId);
                ExistModel.ExpirationTime = DateTime.UtcNow.AddMinutes(TIME_OUT_MINUTES);
                ExistModel.Code = GetNewVerifyCode();
                _recoverPasswordRepository.Update(ExistModel);
            }
            return ExistModel;
        }

        public void VerifyPasswordCode(long userId, string code, bool isRemoveCode = true)
        {
            var verify = _recoverPasswordRepository.GetByUserId(userId);
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
            if (isRemoveCode) _recoverPasswordRepository.Remove(verify);
        }

        private string GetNewVerifyCode()
        {
            Random rand = new Random();
            int num = rand.Next(11111, 99999);
            return num.ToString();
        }
    }
}
