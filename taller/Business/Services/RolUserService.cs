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
using Telegram.Bot.Types;
using Utilities.Exceptions;

namespace Business.Services
{
    public class RolUserService : BusinessBasic<RolUserSelectDto,RolUserDto, RolUser>, IRolUserService
    {

        private readonly IRolUserRepository _dataRolUser;

        private readonly ILogger<RolUserService> _logger;

        public RolUserService(IRolUserRepository data, ILogger<RolUserService> logger, IMapper mapper) : base(data, mapper)
        {
            _dataRolUser = data;
            _logger = logger;

        }

        public async Task<RolUserDto> AsignateUserRTo(int id)
        {
            try
            {
                var entity = await _dataRolUser.AsignateUserRol(id);
                InitializeLogical.InitializeLogicalState(entity); // Inicializa estado lógico (is_deleted = false)
                                                                  // Inicializa estado lógico (is_deleted = false)

                return _mapper.Map<RolUserDto>(entity); 
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al asignar el rol al usuario.", ex);
            }
        }


        public override async Task<IEnumerable<RolUserSelectDto>> GetAllAsync()
        {
            try
            {
                var entity = await _dataRolUser.GetAllAsync();
                return _mapper.Map<IEnumerable<RolUserSelectDto>>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
           
        }

        public  async Task<IEnumerable<string>> GetAllRolUser(int idUser)
        {

            var entity = await _dataRolUser.GetJoinRolesAsync(idUser);
            return entity;
        }

        public async Task<RolUserSelectDto?> GetByIdJoin(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await _dataRolUser.GetByIdAsync(id);
                return entity == null ? default : _mapper.Map<RolUserSelectDto>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
            
        }

    }
}
