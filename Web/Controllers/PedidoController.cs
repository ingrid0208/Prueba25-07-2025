using Entity.DTOs.Default;
using Microsoft.AspNetCore.Mvc;
using Web.Service;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(PedidoService pedidoService, ILogger<PedidoController> logger)
        {
            _pedidoService = pedidoService;
            _logger = logger;
        }

        [HttpPost("crear")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Crear([FromBody] PedidoDto dto)
        {
            try
            {
                await _pedidoService.CreateAsync(dto);
                return Ok(new { isSuccess = true, message = "Pedido registrado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el pedido");
                return BadRequest(new { isSuccess = false, message = ex.Message });
            }
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(List<PedidoDto>), 200)]
        public async Task<IActionResult> Listar()
        {
            var pedidos = await _pedidoService.GetAllAsync();
            return Ok(pedidos);
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> ListarPendientes()
        {
            var pedidos = await _pedidoService.GetByEstadoAsync("Pendiente");
            return Ok(pedidos);
        }

        [HttpPut("entregar/{id}")]
        public async Task<IActionResult> MarcarComoEntregado(int id)
        {
            try
            {
                await _pedidoService.MarcarComoEntregadoAsync(id);
                return Ok(new { isSuccess = true, message = "Pedido entregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message = ex.Message });
            }
        }
    }
}

