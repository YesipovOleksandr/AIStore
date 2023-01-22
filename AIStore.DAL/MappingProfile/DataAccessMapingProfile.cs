using AIStore.DAL.Entities;
using AutoMapper;

namespace AIStore.DAL.MappingProfile
{
    public class DataAccessMapingProfile : Profile
    {
        public DataAccessMapingProfile()
        {
            CreateMap<User, Domain.Models.Users.User>().ReverseMap();
            CreateMap<UserRoles, Domain.Models.Users.UserRoles>().ReverseMap();
            CreateMap<VerifyCode, Domain.Models.Verify.VerifyCode>().ReverseMap();
            CreateMap<RecoverPasswordCode, Domain.Models.RecoverPassword.RecoverPasswordCode>().ReverseMap();
            
        }
    }
}
