using CsvFileSaver_WebApi.Data;
using Microsoft.EntityFrameworkCore;
using static CsvFileSaver_WebApi.Repository.IRepository.IRepository;
using System.Linq.Expressions;
using System.Linq;

namespace CsvFileSaver_WebApi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        //public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
        //    int pageSize = 0, int pageNumber = 1)
        //{
        //    IQueryable<T> query = dbSet;

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }
        //    return await query.ToListAsync();
        //}

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }


    }
}
