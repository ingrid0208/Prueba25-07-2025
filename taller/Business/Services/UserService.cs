using AutoMapper;
using Business.Interfaces.IBusinessImplements;
using Business.Repository;
using Data.Interfaces.IDataImplement;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Default;
using Entity.DTOs.Select;
using Helpers.Business.Business.Helpers.Validation;
using Helpers.Initialize;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services
{
    public class UserService : BusinessBasic<UserSelectDto, UserDto, User>, IUserService
    {
        private readonly IUserRepository _dataUser;
        private readonly ILogger<UserService> _logger;
        private readonly IRolUserService _rolUserService;

        public UserService(
            IUserRepository data,
            ILogger<UserService> logger,
            IMapper mapper,
            IRolUserService rolUserService
        ) : base(data, mapper)
        {
            _dataUser = data;
            _logger = logger;
            _rolUserService = rolUserService;
        }

        // ============================================================
        // ✅ Crear usuario
        // ============================================================
        public async Task<UserDto> CreateAsyncUser(UserDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                var userEntity = _mapper.Map<User>(dto);
                InitializeLogical.InitializeLogicalState(userEntity);

                var createdEntity = await _dataUser.CreateAsync(userEntity);

                // Asignación de rol en background (no bloqueante)
                _ = _rolUserService.AsignateUserRTo(createdEntity.Id);

                return _mapper.Map<UserDto>(createdEntity);
            }
            catch (InvalidOperationException ex)
            {
                // 🚨 Validaciones del helper de negocio
                throw new BusinessException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // 🚨 Excepciones del repositorio o mapeo
                throw new BusinessException("Error al crear el registro.", ex);
            }
        }

        // ============================================================
        // ✅ Actualizar usuario
        // ============================================================
        public async Task<bool> UpdateAsyncUser(UserDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                var entity = _mapper.Map<User>(dto);
                return await _dataUser.UpdateAsync(entity);
            }
            catch (InvalidOperationException ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el registro.", ex);
            }
        }
    }
}
