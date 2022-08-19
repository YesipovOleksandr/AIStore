using AIStore.Api.ViewModels;
using AutoMapper;

namespace AIStore.Api.MappingProfile
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<LoginViewModel, Domain.Models.Users.User>().ReverseMap();
            CreateMap<RegisterViewModel, Domain.Models.Users.User>().ReverseMap();
            CreateMap<AuthViewModel, Domain.Models.Users.User>().ReverseMap();
        }
    }
}
