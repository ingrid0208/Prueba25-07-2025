using Business.Interfaces.IBusinessImplements.Auth;
using Entity.Domain.Config;
using Entity.DTOs.Auth;
using Entity.DTOs.Default;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelSecurity.Controllers.Implements.Auth;
using ModelSecurity.Infrastructure.Cookies.Implements;
using Moq;

namespace test.Controller.Auth
{
    public class AuthControllerTest
    {

        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IToken> _tokenMock;
        private readonly Mock<IAuthCookieFactory> _cookieFactoryMock;
        private readonly IOptions<JwtSettings> _jwtOptions;
        private readonly IOptions<CookieSettings> _cookieOptions;

        private readonly AuthController _controller;

        public AuthControllerTest()
        {
            _loggerMock = new Mock<ILogger<AuthController>>();
            _authServiceMock = new Mock<IAuthService>();
            _tokenMock = new Mock<IToken>();
            _cookieFactoryMock = new Mock<IAuthCookieFactory>();

            _jwtOptions = Options.Create(new JwtSettings
            {
                AccessTokenExpirationMinutes = 15,
                RefreshTokenExpirationDays = 7
            });

            _cookieOptions = Options.Create(new CookieSettings
            {
                AccessTokenName = "access_token",
                RefreshTokenName = "refresh_token",
                CsrfCookieName = "XSRF-TOKEN"
            });

            _controller = new AuthController(
                _loggerMock.Object,
                _tokenMock.Object,
                _authServiceMock.Object,
                _jwtOptions,
                _cookieOptions,
                _cookieFactoryMock.Object
            );
        }

        [Fact]
        public async Task Register_ShouldReturn200_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Email = "usuario@test.com",
                Password = "ClaveSegura123*",
                ConfirmPassword = "ClaveSegura123*",
                FirstName = "Usuario",
                LastName = "Prueba",
                PhoneNumber = "+57 3000000000",
                Identification = "1234567890",
                Address = "Calle Principal 123"
            };

            var fakeUserResult = new UserDto
            {
                Id = 1,
                Email = dto.Email,

            };

            _authServiceMock
                .Setup(s => s.RegisterAsync(dto))
                .ReturnsAsync(fakeUserResult); // Simula creación exitosa

            // Act
            var result = await _controller.Registrarse(dto);

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();

            objectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            objectResult.Value.Should().BeEquivalentTo(new
            {
                isSuccess = true
            });

            _authServiceMock.Verify(s => s.RegisterAsync(dto), Times.Once);
        }

        [Fact]
        public async Task Register_ShouldReturn400_WhenServiceThrowsException()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Email = "repetido@test.com",
                Password = "Clave123*",
                ConfirmPassword = "Clave123*",
                FirstName = "Duplicado",
                LastName = "Usuario",
                PhoneNumber = "+57 3110000000",
                Identification = "1122334455",
                Address = "Avenida 45"
            };

            _authServiceMock
                .Setup(s => s.RegisterAsync(dto))
                .ThrowsAsync(new Exception("El correo ya existe"));

            // Act
            var result = await _controller.Registrarse(dto);

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();

            objectResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            objectResult.Value.Should().BeEquivalentTo(new
            {
                isSuccess = false,
                message = "El correo ya existe"
            });

            _authServiceMock.Verify(s => s.RegisterAsync(dto), Times.Once);
        }
    }
}