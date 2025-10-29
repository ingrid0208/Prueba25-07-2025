using Business.Interfaces.IBusinessImplements.Auth;
using Data.Interfaces.IDataImplement;
using Data.Interfaces.IDataImplement.Auth;
using Entity.Domain.Config;
using Entity.Domain.Models.Auth;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Utilities.Custom;
using Utilities.Exceptions;
using Utilities.Helpers.Jwt;

namespace Business.Custom
{
    /// <summary>
    /// Servicio encargado de gestionar el ciclo de vida de los tokens de autenticación (Access Token, Refresh Token y CSRF Token).
    /// Implementa la lógica de generación, validación, renovación, rotación y revocación de tokens JWT.
    /// </summary>
    public class TokenBusiness : IToken
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userData;
        private readonly IRolUserRepository _rolUserData;
        private readonly ILogger<TokenBusiness> _logger;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="TokenBusiness"/> configurando sus dependencias e inicializando la clave de firma JWT.
        /// </summary>
        /// <param name="configuration">Configuración general de la aplicación.</param>
        /// <param name="userData">Repositorio encargado de la autenticación y obtención de datos de usuario.</param>
        /// <param name="rolUserData">Repositorio encargado de la obtención de roles asociados al usuario.</param>
        /// <param name="logger">Mecanismo de registro de logs para auditoría y diagnóstico.</param>
        /// <param name="refreshToken">Repositorio para la persistencia y gestión de refresh tokens.</param>
        /// <param name="jwtSettings">Configuración del sistema JWT (clave, expiraciones, emisor, audiencia, etc.).</param>
        public TokenBusiness(IConfiguration configuration, IUserRepository userData, IRolUserRepository rolUserData,
            ILogger<TokenBusiness> logger, IRefreshTokenRepository refreshToken, IOptions<JwtSettings> jwtSettings)
        {
            _configuration = configuration;
            _userData = userData;
            _rolUserData = rolUserData;
            _logger = logger;
            _refreshRepo = refreshToken;
            _jwtSettings = jwtSettings.Value;

            EnsureSigningKeyStrength(_jwtSettings.Key);
        }

        /// <summary>
        /// Obtiene los roles asociados a un usuario específico desde el repositorio correspondiente.
        /// </summary>
        /// <param name="idUser">Identificador único del usuario.</param>
        /// <returns>Una colección de nombres de roles asignados al usuario.</returns>
        /// <exception cref="BusinessException">Si ocurre un error al recuperar los roles del usuario.</exception>
        public async Task<IEnumerable<string>> GetRolesUserAsync(int idUser)
        {
            try
            {
                var roles = await _rolUserData.GetRolesUserAsync(idUser);
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles del usuario con ID {UserId}", idUser);
                throw new BusinessException("Error al obtener roles del usuario", ex);
            }
        }

        /// <summary>
        /// Autentica las credenciales del usuario y genera los tokens necesarios para el inicio de sesión:
        /// <list type="bullet">
        /// <item><description><b>Access Token</b>: JWT firmado con información del usuario y sus roles.</description></item>
        /// <item><description><b>Refresh Token</b>: token de larga duración para renovación segura.</description></item>
        /// <item><description><b>CSRF Token</b>: token adicional contra ataques de tipo Cross-Site Request Forgery.</description></item>
        /// </list>
        /// </summary>
        /// <param name="dto">Credenciales del usuario (correo electrónico y contraseña).</param>
        /// <returns>Tupla con el Access Token, Refresh Token y CSRF Token generados.</returns>
        /// <exception cref="BusinessException">Si las credenciales son incorrectas o falla el proceso de generación.</exception>
        public async Task<(string AccessToken, string RefreshToken, string CsrfToken)> GenerateTokensAsync(LoginUserDto dto)
        {
            dto.Password = EncriptePassword.EncripteSHA256(dto.Password);
            var user = await _userData.LoginUser(dto);

            var roles = await _rolUserData.GetRolesUserAsync(user.Id);
            var accessToken = BuildAccessToken(user, roles);

            var now = DateTime.UtcNow;
            var refreshPlain = TokenHelpers.GenerateSecureRandomUrlToken(64);
            var refreshHash = HashRefreshToken(refreshPlain);

            var refreshEntity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshHash,
                CreatedAt = now,
                ExpiresAt = now.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };
            await _refreshRepo.AddAsync(refreshEntity);

            var validTokens = (await _refreshRepo.GetValidTokensByUserAsync(user.Id))
                              .OrderByDescending(t => t.CreatedAt)
                              .ToList();

            const int maxActiveRefreshTokens = 5;
            if (validTokens.Count > maxActiveRefreshTokens)
            {
                foreach (var t in validTokens.Skip(maxActiveRefreshTokens))
                    await _refreshRepo.RevokeAsync(t);
            }

            var csrf = TokenHelpers.GenerateSecureRandomUrlToken(32);

            return (accessToken, refreshPlain, csrf);
        }

        /// <summary>
        /// Intercambia un <b>Refresh Token</b> válido por un nuevo par de tokens (Access y Refresh).
        /// Implementa la rotación segura y la detección de reutilización para mitigar ataques.
        /// </summary>
        /// <param name="refreshTokenPlain">Valor original del Refresh Token proporcionado por el cliente.</param>
        /// <param name="remoteIp">Dirección IP del cliente (opcional, útil para auditoría o trazabilidad).</param>
        /// <returns>Tupla con un nuevo Access Token y un nuevo Refresh Token.</returns>
        /// <exception cref="SecurityTokenException">Si el token ha expirado, fue revocado o es inválido.</exception>
        public async Task<(string NewAccessToken, string NewRefreshToken)> RefreshAsync(string refreshTokenPlain, string remoteIp = null)
        {
            var hash = HashRefreshToken(refreshTokenPlain);
            var record = await _refreshRepo.GetByHashAsync(hash)
                ?? throw new SecurityTokenException("Refresh token inválido");

            if (record.ExpiresAt <= DateTime.UtcNow)
                throw new SecurityTokenException("Refresh token expirado");

            if (record.IsRevoked)
            {
                var validTokens = await _refreshRepo.GetValidTokensByUserAsync(record.UserId);
                foreach (var t in validTokens)
                    await _refreshRepo.RevokeAsync(t);

                throw new SecurityTokenException("Refresh token inválido o reutilizado");
            }

            var user = await _userData.GetByIdAsync(record.UserId)
                ?? throw new SecurityTokenException("Usuario no encontrado");

            var roles = await _rolUserData.GetRolesUserAsync(user.Id);
            var newAccessToken = BuildAccessToken(user, roles);

            var now2 = DateTime.UtcNow;
            var newRefreshPlain = TokenHelpers.GenerateSecureRandomUrlToken(64);
            var newRefreshHash = HashRefreshToken(newRefreshPlain);

            var newRefreshEntity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = newRefreshHash,
                CreatedAt = now2,
                ExpiresAt = now2.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            };

            await _refreshRepo.AddAsync(newRefreshEntity);
            await _refreshRepo.RevokeAsync(record, replacedByTokenHash: newRefreshHash);

            return (newAccessToken, newRefreshPlain);
        }

        /// <summary>
        /// Revoca de forma explícita un <b>Refresh Token</b> evitando su uso futuro en la renovación de sesiones.
        /// </summary>
        /// <param name="refreshToken">Valor original del Refresh Token (en texto plano).</param>
        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var hash = HashRefreshToken(refreshToken);
            var record = await _refreshRepo.GetByHashAsync(hash);
            if (record != null && !record.IsRevoked)
                await _refreshRepo.RevokeAsync(record);
        }

        /// <summary>
        /// Construye un <b>Access Token JWT</b> con los claims básicos del usuario y sus roles.
        /// Incluye información estándar como sub, email, jti, iat y roles asociados.
        /// </summary>
        /// <param name="user">Entidad de usuario autenticado.</param>
        /// <param name="roles">Listado de roles asignados al usuario.</param>
        /// <returns>Cadena JWT firmada lista para enviarse al cliente.</returns>
        private string BuildAccessToken(User user, IEnumerable<string> roles)
        {
            var now = DateTime.UtcNow;
            var accessExp = now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                          new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),
                          ClaimValueTypes.Integer64)
            };

            foreach (var r in roles.Where(r => !string.IsNullOrWhiteSpace(r)).Distinct())
                claims.Add(new Claim(ClaimTypes.Role, r));

            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: now,
                expires: accessExp,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        /// Calcula un hash seguro de un Refresh Token mediante HMAC-SHA512,
        /// usando la clave JWT como pepper para reforzar la seguridad contra ataques de diccionario.
        /// </summary>
        /// <param name="token">Valor original del Refresh Token.</param>
        /// <returns>Cadena hexadecimal en minúsculas con el hash resultante.</returns>
        private string HashRefreshToken(string token)
        {
            var pepper = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            using var hmac = new HMACSHA512(pepper);
            var bytes = Encoding.UTF8.GetBytes(token);
            var mac = hmac.ComputeHash(bytes);
            return Convert.ToHexString(mac).ToLowerInvariant();
        }

        /// <summary>
        /// Verifica que la clave JWT utilizada para firmar los tokens cumpla con un nivel mínimo de entropía.
        /// La clave debe tener al menos 32 caracteres (≈256 bits) para garantizar seguridad criptográfica adecuada.
        /// </summary>
        /// <param name="key">Clave de firma definida en la configuración JWT.</param>
        /// <exception cref="InvalidOperationException">Si la clave es nula, vacía o demasiado corta.</exception>
        private static void EnsureSigningKeyStrength(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || Encoding.UTF8.GetByteCount(key) < 32)
                throw new InvalidOperationException("JwtSettings.Key debe tener al menos 32 caracteres aleatorios (≥256 bits).");
        }
    }
}
