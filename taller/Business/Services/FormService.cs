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
    public class FormService : BusinessBasic<FormSelectDto,FormDto, Form>, IFormService
    {
        private readonly ILogger<FormService> _logger;
        protected readonly IData<Form> Data;
        public FormService(IData<Form> data, IMapper mapper, ILogger<FormService> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }

    }
}
