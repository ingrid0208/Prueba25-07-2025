using AutoMapper;
using Business.Strategy.StrategyDeletes.Implement;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;
using Entity.Domain.Models.Base;
using Helpers.Business.Business.Helpers.Validation;
using Helpers.Initialize;
using Utilities.Exceptions;

namespace Business.Repository
{
    /// <summary>
    /// Clase base abstracta que implementa operaciones CRUD avanzadas con:
    /// - Mapeo DTO-Entidad mediante AutoMapper
    /// - Soporte para estrategias de obtención y eliminación
    /// - Manejo de eliminación lógica y restauración
    /// - Validaciones básicas integradas
    /// </summary>
    /// <typeparam name="TDto">Tipo del DTO para operaciones de creación/actualización</typeparam>
    /// <typeparam name="TDtoGet">Tipo del DTO para operaciones de consulta</typeparam>
    /// <typeparam name="TEntity">Tipo de la entidad de dominio/persistencia</typeparam>
    public class BusinessBasic<TSelect, TCreate, TEntity> : ABaseModelBusiness<TSelect, TCreate, TEntity> where TEntity : BaseModel
    {
        protected readonly IMapper _mapper;
        protected readonly IData<TEntity> Data;

        /// <summary>
        /// Constructor que inicializa las dependencias principales
        /// </summary>
        /// <param name="data">Repositorio de datos para la entidad TEntity</param>
        /// <param name="mapper">Instancia de AutoMapper para transformaciones DTO-Entidad</param>
        public BusinessBasic(IData<TEntity> data, IMapper mapper)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Obtiene todos los registros no eliminados (activos) de la entidad
        /// </summary>
        /// <returns>
        /// Colección de DTOs de consulta (D) mapeados desde las entidades
        /// </returns>
        /// <exception cref="BusinessException">Error durante la operación de base de datos</exception>
        public override async Task<IEnumerable<TSelect>> GetAllAsync()
        {
            try
            {
                var entities = await Data.GetAllAsync();
                return _mapper.Map<IEnumerable<TSelect>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        /// <summary>
        /// Obtiene registros aplicando una estrategia de filtrado específica
        /// </summary>
        /// <param name="getAllType">Estrategia de filtrado a aplicar (enum GetAllType)</param>
        /// <returns>
        /// Colección filtrada de DTOs según la estrategia especificada
        /// </returns>
        /// <exception cref="BusinessException">Error durante la operación o estrategia no soportada</exception>
        public override async Task<IEnumerable<TSelect>> GetAllAsync(GetAllType getAllType)
        {
            try
            {
                var strategy = GetStrategyFactory.GetStrategyGet<TEntity>(Data, getAllType);
                var entities = await strategy.GetAll(Data);
                return _mapper.Map<IEnumerable<TSelect>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener registros con estrategia {getAllType}.", ex);
            }
        }

        /// <summary>
        /// Obtiene un registro específico por su identificador
        /// </summary>
        /// <param name="id">Identificador único del registro (mayor que 0)</param>
        /// <returns>
        /// DTO de consulta (D) o null si no se encuentra el registro
        /// </returns>
        /// <exception cref="InvalidOperationException">Cuando el ID es inválido (≤ 0)</exception>
        /// <exception cref="BusinessException">Error durante la operación de base de datos</exception>
        public override async Task<TSelect?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await Data.GetByIdAsync(id);
                return entity == null ? default : _mapper.Map<TSelect>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo registro en el sistema
        /// </summary>
        /// <param name="dto">DTO con los datos para la creación</param>
        /// <returns>
        /// DTO de creación/actualización (D) con los datos del registro creado
        /// </returns>
        /// <exception cref="InvalidOperationException">Cuando el DTO es nulo o no pasa validaciones</exception>
        /// <exception cref="BusinessException">Error durante la operación de base de datos</exception>
        public override async Task<TCreate> CreateAsync(TCreate dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                var entity = _mapper.Map<TEntity>(dto);
                InitializeLogical.InitializeLogicalState(entity); // Inicializa estado lógico (is_deleted = false)

                var created = await Data.CreateAsync(entity);
                return _mapper.Map<TCreate>(created);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el registro.", ex);
            }
        }

        /// <summary>
        /// Actualiza un registro existente
        /// </summary>
        /// <param name="dto">DTO con los datos actualizados</param>
        /// <returns>
        /// True si la actualización fue exitosa, False si no se encontró el registro
        /// </returns>
        /// <exception cref="InvalidOperationException">Cuando el DTO es nulo o no pasa validaciones</exception>
        /// <exception cref="BusinessException">Error durante la operación de base de datos</exception>
        public override async Task<bool> UpdateAsync(TCreate dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                var entity = _mapper.Map<TEntity>(dto);
                InitializeLogical.InitializeLogicalState(entity); // Inicializa estado lógico (is_deleted = false)

                return await Data.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al actualizar el registro: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina físicamente un registro de la base de datos
        /// </summary>
        /// <param name="id">Identificador único del registro (mayor que 0)</param>
        /// <returns>
        /// True si la eliminación fue exitosa, False si no se encontró el registro
        /// </returns>
        /// <exception cref="InvalidOperationException">Cuando el ID es inválido (≤ 0)</exception>
        /// <exception cref="BusinessException">Error durante la operación de base de datos</exception>
        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");
                return await Data.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el registro con ID {id}.", ex);
            }
        }

        /// <summary>
        /// Elimina un registro aplicando una estrategia específica (física/lógica)
        /// </summary>
        /// <param name="id">Identificador único del registro (mayor que 0)</param>
        /// <param name="deleteType">Tipo de estrategia de eliminación (enum DeleteType)</param>
        /// <returns>
        /// True si la eliminación fue exitosa, False si no se encontró el registro
        /// </returns>
        /// <exception cref="InvalidOperationException">Cuando el ID es inválido (≤ 0)</exception>
        /// <exception cref="BusinessException">Error durante la operación o estrategia no soportada</exception>
        public override async Task<bool> DeleteAsync(int id, DeleteType deleteType)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var strategy = DeleteStrategyFactory.GetStrategy<TEntity>(Data, deleteType);
                return await strategy.DeleteAsync(id, Data);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar registro (ID: {id}) con estrategia {deleteType}.", ex);
            }
        }

        /// <summary>
        /// Restaura un registro que fue eliminado lógicamente (is_deleted = true)
        /// </summary>
        /// <param name="id">Identificador único del registro (mayor que 0)</param>
        /// <returns>
        /// True si la restauración fue exitosa, False si no se encontró el registro
        /// </returns>
        /// <exception cref="InvalidOperationException">Cuando el ID es inválido (≤ 0)</exception>
        /// <exception cref="BusinessException">Error durante la operación de base de datos</exception>
        public override async Task<bool> RestoreLogical(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                return await Data.RestoreAsync(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al restaurar el registro con ID {id}.", ex);
            }
        }


    }
}