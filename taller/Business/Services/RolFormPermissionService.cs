using AutoMapper;
using Business.Interfaces.IBusinessImplements;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.DataBasic;
using Data.Interfaces.IDataImplement;
using Data.Services;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Default;
using Entity.DTOs.Select;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services
{
    public class RolFormPermissionService : BusinessBasic<RolFormPermissionSelectDto,RolFormPermissionDto, RolFormPermission>, IRolFormPermissionService
    {

        private readonly IRolFormPermissionRepository _rolFormPermissionRepository;
        private readonly ILogger<RolFormPermissionService> _logger;
        public RolFormPermissionService(IData<RolFormPermission> data, IMapper mapper, IRolFormPermissionRepository rolFormPermissionRepository, ILogger<RolFormPermissionService> logger) : base(data, mapper)
        {
            _rolFormPermissionRepository = rolFormPermissionRepository;
            _logger = logger;

        }

        public override async Task<IEnumerable<RolFormPermissionSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {

                var strategy = GetStrategyFactory.GetStrategyGet<RolFormPermission>(_rolFormPermissionRepository, getAllType);
                var entities = await strategy.GetAll(_rolFormPermissionRepository);
                return _mapper.Map<IEnumerable<RolFormPermissionSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }
        public override async Task<RolFormPermissionSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await _rolFormPermissionRepository.GetByIdAsync(id);
                return _mapper.Map<RolFormPermissionSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }

        }

        
    }
}
