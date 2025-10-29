using AutoMapper;
using Business.Services;
using Data.Interfaces.IDataImplement;
using Entity.DTOs.Default;
using Entity.DTOs.Select;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Utilities.Exceptions;

namespace test.Business
{
    public class RolUserServiceTests
    {
        private readonly Mock<IRolUserRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<RolUserService>> _mockLogger;
        private readonly RolUserService _service;

        public RolUserServiceTests()
        {
            _mockRepository = new Mock<IRolUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<RolUserService>>();

            _service = new RolUserService(
                _mockRepository.Object,
                _mockLogger.Object,
                _mockMapper.Object
            );
        }

        // ================================================================
        // TEST 1: AsignateUserRTo asigna rol correctamente
        // ================================================================
        [Fact]
        public async Task AsignateUserRToShouldReturnMappedDtoWhenSuccess()
        {
            var rolUser = new RolUser { Id = 1, RolId = 2, UserId = 5, IsDeleted = false };
            var dto = new RolUserDto { Id = 1, RolId = 2, UserId = 5 };

            _mockRepository.Setup(r => r.AsignateUserRol(5)).ReturnsAsync(rolUser);
            _mockMapper.Setup(m => m.Map<RolUserDto>(rolUser)).Returns(dto);

            var result = await _service.AsignateUserRTo(5);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.RolId.Should().Be(2);
            result.UserId.Should().Be(5);
        }

        // ================================================================
        // TEST 2: AsignateUserRTo lanza BusinessException si falla repositorio
        // ================================================================
        [Fact]
        public async Task AsignateUserRToShouldThrowBusinessExceptionWhenRepositoryFails()
        {
            _mockRepository.Setup(r => r.AsignateUserRol(It.IsAny<int>()))
                           .ThrowsAsync(new Exception("DB error"));

            Func<Task> act = async () => await _service.AsignateUserRTo(1);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*Error al asignar el rol al usuario*");
        }

        // ================================================================
        // TEST 3: GetAllAsync retorna correctamente los roles de usuario
        // ================================================================
        [Fact]
        public async Task GetAllAsyncShouldReturnMappedDtos()
        {
            var entities = new List<RolUser>
            {
                new RolUser { Id = 1, RolId = 2, UserId = 10 },
                new RolUser { Id = 2, RolId = 3, UserId = 11 }
            };

            var expectedDtos = new List<RolUserSelectDto>
            {
                new RolUserSelectDto { Id = 1, RolId = 2, UserId = 10 },
                new RolUserSelectDto { Id = 2, RolId = 3, UserId = 11 }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mockMapper.Setup(m => m.Map<IEnumerable<RolUserSelectDto>>(entities))
                       .Returns(expectedDtos);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedDtos);
        }

        // ================================================================
        // TEST 4: GetAllAsync lanza BusinessException si hay error
        // ================================================================
        [Fact]
        public async Task GetAllAsyncShouldThrowBusinessExceptionWhenRepositoryFails()
        {
            _mockRepository.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("DB error"));

            Func<Task> act = async () => await _service.GetAllAsync();

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*Error al obtener todos los registros*");
        }

        // ================================================================
        // TEST 5: GetAllRolUser retorna correctamente los nombres de roles
        // ================================================================
        [Fact]
        public async Task GetAllRolUserShouldReturnRoleNames()
        {
            var roles = new List<string> { "Admin", "User" };
            _mockRepository.Setup(r => r.GetJoinRolesAsync(10)).ReturnsAsync(roles);

            var result = await _service.GetAllRolUser(10);

            result.Should().NotBeNull();
            result.Should().Contain("Admin");
            result.Should().HaveCount(2);
        }

        // ================================================================
        // TEST 6: GetByIdJoin retorna el DTO correctamente
        // ================================================================
        [Fact]
        public async Task GetByIdJoinShouldReturnMappedDtoWhenEntityExists()
        {
            var entity = new RolUser { Id = 5, RolId = 3, UserId = 12 };
            var expectedDto = new RolUserSelectDto { Id = 5, RolId = 3, UserId = 12 };

            _mockRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<RolUserSelectDto>(entity)).Returns(expectedDto);

            var result = await _service.GetByIdJoin(5);

            result.Should().NotBeNull();
            result!.Id.Should().Be(5);
            result.RolId.Should().Be(3);
            result.UserId.Should().Be(12);
        }

        // ================================================================
        // TEST 7: GetByIdJoin lanza excepción si ID es inválido
        // ================================================================
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByIdJoinShouldThrowWhenIdIsInvalid(int invalidId)
        {
            Func<Task> act = async () => await _service.GetByIdJoin(invalidId);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage($"*Error al obtener el registro con ID {invalidId}*");
        }

        // ================================================================
        // TEST 8: GetByIdJoin lanza BusinessException si falla el repositorio
        // ================================================================
        [Fact]
        public async Task GetByIdJoinShouldThrowBusinessExceptionWhenRepositoryFails()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                           .ThrowsAsync(new Exception("DB error"));

            Func<Task> act = async () => await _service.GetByIdJoin(5);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*Error al obtener el registro con ID 5*");
        }
    }
}
