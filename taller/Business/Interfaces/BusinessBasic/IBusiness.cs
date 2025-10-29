using Entity.Domain.Enums;
using System.Dynamic;

namespace Business.Interfaces.BusinessBasic
{
    public interface IBusiness<TSelect, TCreate>
    {
        Task<IEnumerable<TSelect>> GetAllAsync();
        Task<IEnumerable<TSelect>> GetAllAsync(GetAllType g);
        Task<TSelect?> GetByIdAsync(int id);
        Task<TCreate> CreateAsync(TCreate dto);
        Task<bool> UpdateAsync(TCreate dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(int id, DeleteType deleteType);
        Task<bool> RestoreLogical(int id);

    }
}
