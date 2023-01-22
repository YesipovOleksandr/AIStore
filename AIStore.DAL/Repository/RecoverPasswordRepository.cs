using AIStore.DAL.Context;
using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Models.RecoverPassword;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AIStore.DAL.Repository
{
    public class RecoverPasswordRepository : IRecoverPasswordRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public RecoverPasswordRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RecoverPasswordCode> Create(RecoverPasswordCode item)
        {
            var newUser = _mapper.Map<Entities.RecoverPasswordCode>(item);
            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return _mapper.Map<RecoverPasswordCode>(newUser);
        }

        public async Task<bool> ExistUser(long userId)
        {
            bool result = await _context.RecoverPasswordCode.SingleOrDefaultAsync(x => x.UserId == userId) != null;
            return result;
        }

        public RecoverPasswordCode GetByUserId(long Id)
        {
            var item = _mapper.Map<RecoverPasswordCode>(_context.RecoverPasswordCode.AsNoTracking().FirstOrDefault(x => x.UserId == Id));
            return item;
        }

        public void Update(RecoverPasswordCode item)
        {
            _context.Database.ExecuteSqlRaw(@"UPDATE RecoverPasswordCode SET  
            UserId={1}, Code={2},ExpirationTime={3}
            WHERE Id={0}",
          item.Id, item.UserId, item.Code, item.ExpirationTime);
        }

        public void Remove(RecoverPasswordCode item)
        {
            _context.RecoverPasswordCode.Remove(_mapper.Map<Entities.RecoverPasswordCode>(item));
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