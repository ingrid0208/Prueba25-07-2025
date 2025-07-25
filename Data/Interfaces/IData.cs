using System.Linq.Expressions;

namespace Data.Interfaces
{
    public interface IData<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
    }
}
