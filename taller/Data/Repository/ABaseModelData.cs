using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Base;

namespace Data.Repository
{
    public abstract class ABaseModelData<T> : IData<T> where T : BaseModel
    {
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task<IEnumerable<T>> GetDeletes();
        public abstract Task<T?> GetByIdAsync(int id);
        public abstract Task<T> CreateAsync(T entity);
        public abstract Task<bool> UpdateAsync(T entity);
        public abstract Task<bool> DeleteAsync(int id);
        public abstract Task<bool> DeleteLogicAsync(int id);
        public abstract Task<bool> RestoreAsync(int id);
    }
}
