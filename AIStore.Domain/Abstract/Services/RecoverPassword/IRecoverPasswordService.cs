using AIStore.Domain.Models.RecoverPassword;
using AIStore.Domain.Models.Verify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStore.Domain.Abstract.Services.RecoverPassword
{
    public interface IRecoverPasswordService
    {
        Task<RecoverPasswordCode> SetRecoverPasswordCode(long userId);
        void VerifyPasswordCode(long userId, string code, bool isRemoveCode = true);
    }
}
