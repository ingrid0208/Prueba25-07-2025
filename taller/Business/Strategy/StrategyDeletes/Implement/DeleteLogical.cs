using Business.Strategy.StrategyDeletes.Interfaces;
using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;

namespace Business.Strategy.StrategyDeletes.Implement
{
    public class DeleteLogical<T> : IStrategyDeletes<T> where T : class
    {
        public DeleteType Type => DeleteType.Logical;
        public async Task<bool> DeleteAsync(int id, IData<T> repository)
        {
            // Aseguramos que el repositorio implemente IDataExtend<T>
            return await repository.DeleteLogicAsync(id);
        }
    }
}
