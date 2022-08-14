using AIStore.DAL.Entities;
using AutoMapper;

namespace AIStore.DAL.MappingProfile
{
    public class DataAccessMapingProfile : Profile
    {
        public DataAccessMapingProfile()
        {
            CreateMap<User, Domain.Models.Users.User>().ReverseMap();
        }
    }
}
