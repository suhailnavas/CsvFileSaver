using System.Linq.Expressions;

namespace CsvFileSaver_WebApi.Repository.IRepository
{
    public interface IRepository
    {
        public interface IRepository<T> where T : class
        {           
            Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
            Task CreateAsync(T entity);
            Task RemoveAsync(T entity);
            Task SaveAsync();
        }
    }
}
