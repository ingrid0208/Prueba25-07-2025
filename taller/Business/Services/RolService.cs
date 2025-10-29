

using AutoMapper;
using Business.Interfaces.IBusinessImplements;
using Business.Repository;
using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Default;
using Entity.DTOs.Select;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class RolService : BusinessBasic<RolSelectDto,RolDto , Rol>, IRolService
    {

        private readonly ILogger<RolService> _logger;
        protected readonly IData<Rol> Data;

        public RolService(IData<Rol> data, IMapper mapper, ILogger<RolService> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }

    }
}
