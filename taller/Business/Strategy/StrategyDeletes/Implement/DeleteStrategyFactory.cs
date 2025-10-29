using Business.Strategy.StrategyDeletes.Interfaces;
using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;

namespace Business.Strategy.StrategyDeletes.Implement
{
    public static class DeleteStrategyFactory
    {
        public static IStrategyDeletes<T> GetStrategy<T>(IData<T> repository, DeleteType deleteType) where T : class
        {
            return deleteType switch
            {
                DeleteType.Logical when repository is IData<T> => new DeleteLogical<T>(),
                DeleteType.Persistent => new DeletePersistent<T>(),
                _ => throw new InvalidOperationException("Tipo de borrado no soportado para este repositorio.")
            };
        }
    }
}
