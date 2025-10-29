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
    public class PermissionService : BusinessBasic<PermissionSelectDto,PermissionDto, Permission>, IPermissionService
    {
        private readonly ILogger<PermissionService> _logger;
        protected readonly IData<Permission> Data;
        public PermissionService(IData<Permission> data, IMapper mapper, ILogger<PermissionService> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }

    }
}
