using Entity.DTOs.Default;
using Microsoft.AspNetCore.Mvc;
using Web.Service;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly PizzaService _pizzaService;
        private readonly ILogger<PizzaController> _logger;

        public PizzaController(PizzaService pizzaService, ILogger<PizzaController> logger)
        {
            _pizzaService = pizzaService;
            _logger = logger;
        }

        [HttpPost("crear")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Crear([FromBody] PizzaDto dto)
        {
            try
            {
                await _pizzaService.CreateAsync(dto);
                return Ok(new { isSuccess = true, message = "Pizza registrada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la pizza");
                return BadRequest(new { isSuccess = false, message = ex.Message });
            }
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(List<PizzaDto>), 200)]
        public async Task<IActionResult> Listar()
        {
            var pizzas = await _pizzaService.GetAllAsync();
            return Ok(pizzas);
        }
    }
}


