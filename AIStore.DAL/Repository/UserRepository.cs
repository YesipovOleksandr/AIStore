using AIStore.DAL.Context;
using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Models.Users;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

        public async Task<User> Create(User item)
        {
            var newUser = _mapper.Map<Entities.User>(item);
            _context.Add(newUser);
            await _context.SaveChangesAsync();

            return _mapper.Map<User>(newUser);
        }
        public async Task<User> GetById(long Id)
        { 
            var user = _mapper.Map<User>(await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(x => x.Id == Id));
            return user;
        }

        public async Task<User> GetByLogin(string login)
        {
            var user =  _mapper.Map<User>(await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(x => x.Login == login));

            return user;
        }

        public async Task Update(User item)
        {
            await _context.Database.ExecuteSqlRawAsync(@"UPDATE Users SET  
            Login={1}, Password={2},RefreshToken={3},RefreshTokenExpiryTime={4},IsEmailСonfirm={5}
            WHERE Id={0}",
            item.Id, item.Login, item.Password, item.RefreshToken, item.RefreshTokenExpiryTime, item.IsEmailСonfirm);
            await _context.SaveChangesAsync();
        }
        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
