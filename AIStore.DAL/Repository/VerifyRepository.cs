using AIStore.DAL.Context;
using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Models.Verify;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AIStore.DAL.Repository
{
    public class VerifyRepository : IVerifyRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public VerifyRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VerifyCode> Create(VerifyCode item)
        {
            var newUser = _mapper.Map<Entities.VerifyCode>(item);
            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return _mapper.Map<VerifyCode>(newUser);
        }

        public async Task<bool> ExistUser(long userId)
        {
            bool result = await _context.VerifyCode.SingleOrDefaultAsync(x => x.UserId == userId) != null;
            return result;
        }

        public VerifyCode GetById(long Id)
        {
            var user = _mapper.Map<VerifyCode>(_context.VerifyCode.FirstOrDefault(x => x.Id == Id));
            return user;
        }

        public VerifyCode GetByUserId(long Id)
        {
            var user = _mapper.Map<VerifyCode>(_context.VerifyCode.AsNoTracking().FirstOrDefault(x => x.UserId == Id));
            return user;
        }

        public void Update(VerifyCode item)
        {
            _context.Database.ExecuteSqlRaw(@"UPDATE VerifyCode SET  
            UserId={1}, Code={2},ExpirationTime={3}
            WHERE Id={0}",
          item.Id, item.UserId, item.Code, item.ExpirationTime);
        }

        public void Remove(VerifyCode item)
        {
            _context.VerifyCode.Remove(_mapper.Map<Entities.VerifyCode>(item));
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
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
