using Entity.DTOs.Default;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Service;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] ClienteDto dto)
        {
            await _clienteService.CreateAsync(dto);
            return Ok(new { message = "Cliente registrado" });
        }
    }
}


