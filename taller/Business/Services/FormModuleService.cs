using AutoMapper;
using Business.Interfaces.IBusinessImplements;
using Business.Repository;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.DataBasic;
using Data.Interfaces.IDataImplement;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Default;
using Entity.DTOs.Select;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services
{
    public class FormModuleService : BusinessBasic<FormModuleSelectDto,FormModuleDto,FormModule>, IFormModuleService
    {
        private readonly IFormModuleRepository _formModuleRepository;
        private readonly ILogger<FormModuleService> _logger;

        public FormModuleService(IData<FormModule> data, IMapper mapper, IFormModuleRepository formModuleRepository, ILogger<FormModuleService> logger) : base(data, mapper)
        {
            _formModuleRepository = formModuleRepository;
            _logger = logger;
        }

        public override async Task<IEnumerable<FormModuleSelectDto>> GetAllAsync(GetAllType getAllType)
        {
            try
            {

                var strategy = GetStrategyFactory.GetStrategyGet<FormModule>(_formModuleRepository, getAllType);
                var entities = await strategy.GetAll(_formModuleRepository);
                return _mapper.Map<IEnumerable<FormModuleSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener todos los registros.", ex);
            }
        }

        public override async Task<FormModuleSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await _formModuleRepository.GetByIdAsync(id);
                return _mapper.Map<FormModuleSelectDto?>(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
            
        }
    }
}
