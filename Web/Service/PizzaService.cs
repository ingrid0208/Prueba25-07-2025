using AutoMapper;
using Business.Repository;
using Data.Interfaces;
using Entity.DTOs.Default;
using Entity.Models;
using Utilities.Exceptions;

namespace Web.Service
{
    public class PizzaService : BusinessBasic<PizzaDto, Pizza>
    {
        private readonly IData<Pizza> _pizzaRepository;
        private readonly ILogger<PizzaService> _logger;

        public PizzaService(IData<Pizza> data, ILogger<PizzaService> logger, IMapper mapper)
            : base(data, mapper)
        {
            _pizzaRepository = data;
            _logger = logger;
        }

        protected override void ValidateDto(PizzaDto dto)
        {
            if (dto == null)
                throw new ValidationException("La pizza no puede ser nula");

            if (string.IsNullOrWhiteSpace(dto.NombreProducto))
                throw new ValidationException("El nombre de la pizza es obligatorio");

            if (dto.Precio <= 0)
                throw new ValidationException("El precio debe ser mayor a 0");
        }

        protected override async Task ValidateIdAsync(int id)
        {
            var pizza = await _pizzaRepository.GetByIdAsync(id);
            if (pizza == null)
                throw new EntityNotFoundException($"No existe la pizza con ID {id}");
        }
    }
}

 