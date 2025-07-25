using AutoMapper;
using Business.Repository;
using Data.Interfaces;
using Entity.DTOs.Default;
using Entity.Models;
using Utilities.Exceptions;

namespace Web.Service
{
    public class PedidoService : BusinessBasic<PedidoDto, Pedido>
    {
        private readonly IData<Pedido> _pedidoRepository;
        private readonly ILogger<PedidoService> _logger;

        public PedidoService(IData<Pedido> data, ILogger<PedidoService> logger, IMapper mapper)
            : base(data, mapper)
        {
            _pedidoRepository = data;
            _logger = logger;
        }

        protected override void ValidateDto(PedidoDto dto)
        {
            if (dto == null)
                throw new ValidationException("El pedido no puede ser nulo");

            if (dto.IdCliente <= 0)
                throw new ValidationException("Debe especificar el cliente");

            if (dto.IdUsuario <= 0)
                throw new ValidationException("Debe especificar el usuario que lo registra");
        }

        protected override async Task ValidateIdAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                throw new EntityNotFoundException($"No existe el pedido con ID {id}");
        }

        public async Task<IEnumerable<PedidoDto>> GetByEstadoAsync(string estado)
        {
            var pedidos = await _pedidoRepository.GetAsync(p => p.Estado == estado);
            return _mapper.Map<IEnumerable<PedidoDto>>(pedidos);
        }

        public async Task MarcarComoEntregadoAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                throw new EntityNotFoundException($"Pedido con ID {id} no encontrado");

            pedido.Estado = "Entregado";
            await _pedidoRepository.UpdateAsync(pedido);
        }
    }
}
