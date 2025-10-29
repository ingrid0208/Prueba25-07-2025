using AutoMapper;
using Business.Interfaces.IBusinessImplements;
using Business.Repository;
using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Default;
using Entity.DTOs.Select;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business.Services
{
    public class ModuleService : BusinessBasic<ModuleSelectDto, ModuleDto, Module>, IModuleService
    {
        private readonly ILogger<ModuleService> _logger;

        protected readonly IData<Module> Data;
        public ModuleService(IData<Module> data, IMapper mapper, ILogger<ModuleService> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }


    }
}
