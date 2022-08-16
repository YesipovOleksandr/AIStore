using AIStore.DAL.Context;
using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Models.Users;
using AutoMapper;

namespace AIStore.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public User Create(User user)
        {
            var newUser = _mapper.Map<Entities.User>(user);
            _context.Add(newUser);
            _context.SaveChanges();

            return _mapper.Map<User>(newUser);
        }

        public User Get(string login)
        {
            var user = _mapper.Map<User>(_context.Users.FirstOrDefault(x => x.Login == login));

            return user;
        }
    }
}
