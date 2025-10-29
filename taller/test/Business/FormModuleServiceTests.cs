using AutoMapper;
using Business.Services;
using Business.Strategy.StrategyGet.Implement;
using Data.Interfaces.DataBasic;
using Data.Interfaces.IDataImplement;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Select;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Utilities.Exceptions;

namespace test.Business
{
    public class FormModuleServiceTests
    {
        private readonly Mock<IData<FormModule>> _mockData;
        private readonly Mock<IFormModuleRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<FormModuleService>> _mockLogger;
        private readonly FormModuleService _service;

        public FormModuleServiceTests()
        {
            _mockData = new Mock<IData<FormModule>>();
            _mockRepository = new Mock<IFormModuleRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<FormModuleService>>();

            _service = new FormModuleService(
                _mockData.Object,
                _mockMapper.Object,
                _mockRepository.Object,
                _mockLogger.Object
            );
        }

        // ================================================================
        // TEST 1: GetAllAsync retorna los registros correctamente
        // ================================================================
        [Fact]
        public async Task GetAllAsyncShouldReturnMappedDtos()
        {
            // Arrange
            var entities = new List<FormModule>
            {
                new FormModule { Id = 1, FormId = 10, ModuleId = 20 },
                new FormModule { Id = 2, FormId = 11, ModuleId = 21 }
            };

            var expectedDtos = new List<FormModuleSelectDto>
            {
                new FormModuleSelectDto { Id = 1, FormId = 10, ModuleId = 20 },
                new FormModuleSelectDto { Id = 2, FormId = 11, ModuleId = 21 }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mockMapper.Setup(m => m.Map<IEnumerable<FormModuleSelectDto>>(entities))
                       .Returns(expectedDtos);

            var result = await _service.GetAllAsync(GetAllType.GetAll);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedDtos);
        }

        // ================================================================
        // TEST 2: GetAllAsync lanza BusinessException si hay error
        // ================================================================
        [Fact]
        public async Task GetAllAsyncShouldThrowBusinessExceptionWhenRepositoryFails()
        {
            _mockRepository.Setup(r => r.GetAllAsync())
                           .ThrowsAsync(new Exception("DB connection error"));

            Func<Task> act = async () => await _service.GetAllAsync(GetAllType.GetAll);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*Error al obtener todos los registros*");
        }

        // ================================================================
        // TEST 3: GetByIdAsync retorna un registro mapeado correctamente
        // ================================================================
        [Fact]
        public async Task GetByIdAsyncShouldReturnMappedDtoWhenEntityExists()
        {
            var entity = new FormModule { Id = 5, FormId = 15, ModuleId = 25 };
            var expectedDto = new FormModuleSelectDto { Id = 5, FormId = 15, ModuleId = 25 };

            _mockRepository.Setup(r => r.GetByIdAsync(5))
                           .ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<FormModuleSelectDto?>(entity))
                       .Returns(expectedDto);

            var result = await _service.GetByIdAsync(5);

            result.Should().NotBeNull();
            result!.Id.Should().Be(5);
            result.FormId.Should().Be(15);
            result.ModuleId.Should().Be(25);
        }

        // ================================================================
        // TEST 4: GetByIdAsync lanza excepción si ID es inválido
        // ================================================================
        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task GetByIdAsyncShouldThrowWhenIdIsInvalid(int invalidId)
        {
            Func<Task> act = async () => await _service.GetByIdAsync(invalidId);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage($"*Error al obtener el registro con ID {invalidId}*");
        }

        // ================================================================
        // TEST 5: GetByIdAsync lanza BusinessException si ocurre error interno
        // ================================================================
        [Fact]
        public async Task GetByIdAsyncShouldThrowBusinessExceptionWhenRepositoryFails()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                           .ThrowsAsync(new Exception("DB fail"));

            Func<Task> act = async () => await _service.GetByIdAsync(10);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*Error al obtener el registro con ID 10*");
        }
    }
}
