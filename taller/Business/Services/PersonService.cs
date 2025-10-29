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
    public class PersonService : BusinessBasic<PersonSelectDto,PersonDto, Person>, IPersonService
    {
        private readonly ILogger<PersonService> _logger;
        protected readonly IData<Person> Data;
        public PersonService(IData<Person> data, IMapper mapper, ILogger<PersonService> logger) : base(data, mapper)
        {
            Data = data;
            _logger = logger;
        }

    }
}
