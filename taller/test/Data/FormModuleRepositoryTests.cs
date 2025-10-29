using Data.Services;
using Entity.Domain.Models.Implements;
using Entity.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace test.Data
{
    public class FormModuleRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly FormModuleRepository _repository;

        public FormModuleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // base única por test
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new FormModuleRepository(_context);

            // Entidades relacionadas mínimas (necesarias por los Include)
            var form1 = new Form { Id = 10, Name = "Form A", Description = "Desciption" };
            var form2 = new Form { Id = 11, Name = "Form B", Description = "Desciption" };
            var form3 = new Form { Id = 12, Name = "Form C", Description = "Desciption" };

            var module1 = new Module { Id = 100, Name = "Module A", Description = "Desciption" };
            var module2 = new Module { Id = 101, Name = "Module B", Description = "Desciption" };
            var module3 = new Module { Id = 102, Name = "Module C", Description = "Desciption" };

            _context.Forms.AddRange(form1, form2, form3);
            _context.Modules.AddRange(module1, module2, module3);

            // Asociaciones (FormModule)
            _context.FormModules.AddRange(
                new FormModule { Id = 1, IsDeleted = false, FormId = 10, ModuleId = 100 },
                new FormModule { Id = 2, IsDeleted = true, FormId = 11, ModuleId = 101 },
                new FormModule { Id = 3, IsDeleted = false, FormId = 12, ModuleId = 102 }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnOnlyActiveRecords()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(x => !x.IsDeleted).Should().BeTrue();
        }

        [Fact]
        public async Task GetDeletesShouldReturnOnlyDeletedRecords()
        {
            // Act
            var result = await _repository.GetDeletes();

            // Assert
            result.Should().NotBeNull();
            result.Should().ContainSingle(x => x.IsDeleted);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnEntityWhenExists()
        {
            // Act
            var result = await _repository.GetByIdAsync(3);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(3);
            result.FormId.Should().Be(12);
            result.ModuleId.Should().Be(102);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullWhenNotExists()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }
    }
}
