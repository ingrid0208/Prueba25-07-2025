using AutoMapper;
using Business.Repository;
using Data.Interfaces;
using Entity.DTOs.Default;
using Entity.Models;
using Utilities.Exceptions;

namespace Web.Service
{
    public class ClienteService : BusinessBasic<ClienteDto, Cliente>
    {
        private readonly IData<Cliente> _clienteRepository;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IData<Cliente> data, ILogger<ClienteService> logger, IMapper mapper)
            : base(data, mapper)
        {
            _clienteRepository = data;
            _logger = logger;
        }

        protected override void ValidateDto(ClienteDto dto)
        {
            if (dto == null)
                throw new ValidationException("El cliente no puede ser nulo");

            if (string.IsNullOrWhiteSpace(dto.first_name))
                throw new ValidationException("El nombre del cliente es obligatorio");

            if (string.IsNullOrWhiteSpace(dto.last_name))
                throw new ValidationException("El apellido del cliente es obligatorio");
        }

        protected override async Task ValidateIdAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
                throw new EntityNotFoundException($"No existe el cliente con ID {id}");
        }
    }
}