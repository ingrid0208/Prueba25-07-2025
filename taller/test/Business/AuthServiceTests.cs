using AutoMapper;
using Business.Services.Auth;
using Data.Interfaces.IDataImplement;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Auth;
using Entity.DTOs.Default;
using FluentAssertions;
using Helpers.Business.Business.Helpers.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Utilities.Exceptions;

namespace test.Business.Auth
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IRolUserRepository> _mockRolUserRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockRolUserRepo = new Mock<IRolUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            _service = new AuthService(
                _mockUserRepo.Object,
                _mockLogger.Object,
                _mockRolUserRepo.Object,
                _mockMapper.Object
            );
        }

        // ===================================================
        // TEST 1: Registro exitoso
        // ===================================================
        [Fact]
        public async Task RegisterAsyncShouldRegisterUserSuccessfully()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                FirstName = "Juan",
                LastName = "Manuel",
                Address = "Calle",
                Identification = "9999",
                Email = "test@mail.com",
                Password = "StrongPassword123!",
                ConfirmPassword = "StrongPassword123!",
            };

            var person = new Person { Id = 1, Identification = "9999", FirstName = "Juan", LastName = "Manuel", Address = "Calle" };
            var user = new User { Id = 1, Email = dto.Email, Password = dto.Password, Person = person };

            _mockUserRepo.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
            _mockUserRepo.Setup(r => r.ExistsByDocumentAsync(dto.Identification)).ReturnsAsync(false);
            _mockMapper.Setup(m => m.Map<Person>(dto)).Returns(person);
            _mockMapper.Setup(m => m.Map<User>(dto)).Returns(user);
            _mockUserRepo.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(user);
            _mockUserRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { Email = dto.Email });
            _mockRolUserRepo.Setup(r => r.AsignateUserRol(user.Id)).ReturnsAsync(new RolUser());

            // Act
            var result = await _service.RegisterAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email);

            _mockUserRepo.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
            _mockRolUserRepo.Verify(r => r.AsignateUserRol(user.Id), Times.AtLeastOnce);
        }

        // ===================================================
        // TEST 2: Contraseñas no coinciden
        // ===================================================
        [Fact]
        public async Task RegisterAsyncShouldThrowWhenPasswordsDoNotMatch()
        {
            var dto = new RegisterUserDto
            {
                Email = "test@mail.com",
                Password = "abc",
                ConfirmPassword = "xyz"
            };

            Func<Task> act = async () => await _service.RegisterAsync(dto);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*contraseñas no coinciden*");
        }

        // ===================================================
        // TEST 3: Correo ya registrado
        // ===================================================
        [Fact]
        public async Task RegisterAsyncShouldThrowWhenEmailAlreadyExists()
        {
            var dto = new RegisterUserDto
            {
                Email = "test@mail.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _mockUserRepo.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(true);

            Func<Task> act = async () => await _service.RegisterAsync(dto);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*Correo ya registrado*");
        }

        // ===================================================
        // TEST 4: Documento duplicado
        // ===================================================
        [Fact]
        public async Task RegisterAsyncShouldThrowWhenDocumentAlreadyExists()
        {
            var dto = new RegisterUserDto
            {
                Email = "test@mail.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Identification = "12345"
            };

            _mockUserRepo.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
            _mockUserRepo.Setup(r => r.ExistsByDocumentAsync(dto.Identification)).ReturnsAsync(true);

            Func<Task> act = async () => await _service.RegisterAsync(dto);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*numero de identificacion*");
        }

        // ===================================================
        // TEST 5: Contraseña inválida
        // ===================================================
        [Fact]
        public async Task RegisterAsyncShouldThrowWhenPasswordIsWeak()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Email = "test@mail.com",
                Password = "abc",
                ConfirmPassword = "abc"
            };

            _mockUserRepo.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
            _mockUserRepo.Setup(r => r.ExistsByDocumentAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _service.RegisterAsync(dto);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*Contraseña no valida*");
        }

        // ===================================================
        // TEST 6: Usuario no recuperado tras creación
        // ===================================================
        [Fact]
        public async Task RegisterAsyncShouldThrowWhenUserNotRetrievedAfterCreation()
        {
            var dto = new RegisterUserDto
            {
                Email = "test@mail.com",
                Password = "StrongPassword123!",
                ConfirmPassword = "StrongPassword123!",
                Identification = "9999"
            };

            var person = new Person { Id = 1, Identification = dto.Identification };
            var user = new User { Id = 1, Email = dto.Email, Password = dto.Password };

            _mockUserRepo.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
            _mockUserRepo.Setup(r => r.ExistsByDocumentAsync(dto.Identification)).ReturnsAsync(false);

            _mockMapper.Setup(m => m.Map<Person>(dto)).Returns(person);
            _mockMapper.Setup(m => m.Map<User>(dto)).Returns(user);

            _mockUserRepo.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(user);
            _mockUserRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync((User?)null);

            Func<Task> act = async () => await _service.RegisterAsync(dto);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*no pudo ser recuperado*");
        }
    }
}
