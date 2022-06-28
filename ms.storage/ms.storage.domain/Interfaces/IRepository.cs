using ms.storage.domain.Entities;
using System.Linq.Expressions;

namespace ms.storage.domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task<bool> SaveChangesAsync();
        Task DeleteAsync(int id);
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);
    }
}
