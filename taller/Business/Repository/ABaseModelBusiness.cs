using Business.Interfaces.BusinessBasic;
using Entity.Domain.Enums;
using Entity.Domain.Models.Base;

namespace Business.Repository
{
    public abstract class ABaseModelBusiness<TSelect, TCreate, TEntity> : IBusiness<TSelect, TCreate> where TEntity : BaseModel
    {

        public abstract Task<IEnumerable<TSelect>> GetAllAsync();  
        public abstract Task<IEnumerable<TSelect>> GetAllAsync(GetAllType g);
        public abstract Task<TSelect?> GetByIdAsync(int id);
        public abstract Task<TCreate> CreateAsync(TCreate dto);
        public abstract Task<bool> UpdateAsync(TCreate dto);
        public abstract Task<bool> DeleteAsync(int id);
        public abstract Task<bool> DeleteAsync(int id, DeleteType deleteType);
        public abstract Task<bool> RestoreLogical(int id);



 
    }
}
