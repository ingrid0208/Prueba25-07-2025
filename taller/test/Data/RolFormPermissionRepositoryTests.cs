using Data.Services;
using Entity.Domain.Models.Implements;
using Entity.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace test.Data
{
    public class RolFormPermissionRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly RolFormPermissionRepository _repository;

        public RolFormPermissionRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new RolFormPermissionRepository(_context);

            // Datos base relacionados
            var rol1 = new Rol { Id = 1, Name = "Admin", Description = "Description" };
            var rol2 = new Rol { Id = 2, Name = "User", Description = "Description" };

            var form1 = new Form { Id = 1, Name = "Dashboard", Description = "Description" };
            var form2 = new Form { Id = 2, Name = "Settings", Description = "Description" };

            var perm1 = new Permission { Id = 1, Name = "View", Description = "Description" };
            var perm2 = new Permission { Id = 2, Name = "Edit", Description = "Description" };

            _context.Rols.AddRange(rol1, rol2);
            _context.Forms.AddRange(form1, form2);
            _context.Permissions.AddRange(perm1, perm2);

            _context.RolFormPermissions.AddRange(
                new RolFormPermission
                {
                    Id = 1,
                    RolId = 1,
                    FormId = 1,
                    PermissionId = 1,
                    IsDeleted = false
                },
                new RolFormPermission
                {
                    Id = 2,
                    RolId = 1,
                    FormId = 2,
                    PermissionId = 2,
                    IsDeleted = true
                },
                new RolFormPermission
                {
                    Id = 3,
                    RolId = 2,
                    FormId = 1,
                    PermissionId = 2,
                    IsDeleted = false
                }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveRecords_WithRelations()
        {
            var result = await _repository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(x => !x.IsDeleted).Should().BeTrue();

            // Validar includes (relaciones cargadas)
            result.First().Rol.Should().NotBeNull();
            result.First().Form.Should().NotBeNull();
            result.First().Permission.Should().NotBeNull();
        }

        [Fact]
        public async Task GetDeletes_ShouldReturnOnlyDeletedRecords_WithRelations()
        {
            var result = await _repository.GetDeletes();

            result.Should().NotBeNull();
            result.Should().ContainSingle(x => x.IsDeleted);
            result.First().Rol.Should().NotBeNull();
            result.First().Form.Should().NotBeNull();
            result.First().Permission.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
        {
            var result = await _repository.GetByIdAsync(3);

            result.Should().NotBeNull();
            result!.Id.Should().Be(3);
            result.Rol.Should().NotBeNull();
            result.Form.Should().NotBeNull();
            result.Permission.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var result = await _repository.GetByIdAsync(999);
            result.Should().BeNull();
        }
    }
}
