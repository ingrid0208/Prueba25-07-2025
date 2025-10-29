using Business.Interfaces.IBusinessImplements.Auth;
using Entity.Domain.Config;
using Entity.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModelSecurity.Infrastructure.Cookies.Implements;

namespace ModelSecurity.Controllers.Implements.Auth
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly IToken _token;
        private readonly IAuthService _authService;
        private readonly JwtSettings _jwt;
        private readonly CookieSettings _cookieSettings;
        private readonly IAuthCookieFactory _cookieFactory;


        public AuthController(ILogger<AuthController> logger,
            IToken token,
            IAuthService authService,
            IOptions<JwtSettings> jwtOptions,
            IOptions<CookieSettings> cookieOptions,
            IAuthCookieFactory cookieFactory)
        {

            _logger = logger;
            _token = token;
            _jwt = jwtOptions.Value;
            _cookieSettings = cookieOptions.Value;
            _cookieFactory = cookieFactory;
            _authService = authService;

        }


        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Registrarse(RegisterUserDto objeto)
        {
            try
            {
                var userCreated = await _authService.RegisterAsync(objeto);

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { isSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto, CancellationToken ct)
        {
            try
            {
                var (access, refresh, csrf) = await _token.GenerateTokensAsync(dto);

                var now = DateTime.UtcNow;

                Response.Cookies.Append(
                    _cookieSettings.AccessTokenName,
                    access,
                    _cookieFactory.AccessCookieOptions(now.AddMinutes(_jwt.AccessTokenExpirationMinutes)));

                Response.Cookies.Append(
                    _cookieSettings.RefreshTokenName,
                    refresh,
                    _cookieFactory.RefreshCookieOptions(now.AddDays(_jwt.RefreshTokenExpirationDays)));

                Response.Cookies.Append(
                    _cookieSettings.CsrfCookieName,
                    csrf,
                    _cookieFactory.CsrfCookieOptions(now.AddDays(_jwt.RefreshTokenExpirationDays)));

                return Ok(new { isSuccess = true, message = "Login exitoso" });
            }
            catch (UnauthorizedAccessException)
            {
                // Mensaje controlado y status 401
                return Unauthorized(new { isSuccess = false, message = "Credenciales inválidas" });
            }
        }



        /// <summary>Renueva tokens (usa refresh cookie + comprobación CSRF double-submit).</summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            var refreshCookie = Request.Cookies[_cookieSettings.RefreshTokenName];
            if (string.IsNullOrWhiteSpace(refreshCookie))
                return Unauthorized();

            // Validación CSRF (double-submit: header debe igualar cookie)
            if (!Request.Headers.TryGetValue("X-XSRF-TOKEN", out var headerValue))
                return Forbid();

            var csrfCookie = Request.Cookies[_cookieSettings.CsrfCookieName];
            if (string.IsNullOrWhiteSpace(csrfCookie) || csrfCookie != headerValue)
                return Forbid();

            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var (newAccess, newRefresh) = await _token.RefreshAsync(refreshCookie, remoteIp);

            var now = DateTime.UtcNow;

            // Eliminar cookies anteriores con las MISMAS opciones (path/domain/samesite) para asegurar borrado
            Response.Cookies.Delete(_cookieSettings.AccessTokenName, _cookieFactory.AccessCookieOptions(now));
            Response.Cookies.Delete(_cookieSettings.RefreshTokenName, _cookieFactory.RefreshCookieOptions(now));

            // Escribir nuevas
            Response.Cookies.Append(
                _cookieSettings.AccessTokenName,
                newAccess,
                _cookieFactory.AccessCookieOptions(now.AddMinutes(_jwt.AccessTokenExpirationMinutes)));

            Response.Cookies.Append(
                _cookieSettings.RefreshTokenName,
                newRefresh,
                _cookieFactory.RefreshCookieOptions(now.AddDays(_jwt.RefreshTokenExpirationDays)));

            return Ok(new { isSuccess = true });
        }


        /// <summary>Logout: revoca refresh token y borra cookies.</summary>
        [HttpPost("logout")]
        [AllowAnonymous] // puede hacerse sin estar autenticado si solo borra cookies
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            var refreshCookie = Request.Cookies[_cookieSettings.RefreshTokenName];
            if (!string.IsNullOrWhiteSpace(refreshCookie))
            {
                await _token.RevokeRefreshTokenAsync(refreshCookie);
            }

            var now = DateTime.UtcNow;
            Response.Cookies.Delete(_cookieSettings.AccessTokenName, _cookieFactory.AccessCookieOptions(now));
            Response.Cookies.Delete(_cookieSettings.RefreshTokenName, _cookieFactory.RefreshCookieOptions(now));
            Response.Cookies.Delete(_cookieSettings.CsrfCookieName, _cookieFactory.CsrfCookieOptions(now));

            return Ok(new { message = "Sesión cerrada" });
        }

        /// <summary>Revoca el refresh token actual (si existe) y elimina la cookie.</summary>
        [HttpPost("revoke-token")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokeToken()
        {
            var refreshToken = Request.Cookies[_cookieSettings.RefreshTokenName];
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest(new { message = "No hay refresh token" });

            await _token.RevokeRefreshTokenAsync(refreshToken);

            var now = DateTime.UtcNow;
            Response.Cookies.Delete(_cookieSettings.RefreshTokenName, _cookieFactory.RefreshCookieOptions(now));

            return Ok(new { message = "Refresh token revocado" });
        }




    }
}