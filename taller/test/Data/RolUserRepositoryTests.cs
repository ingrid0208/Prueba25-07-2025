using Data.Services;
using Entity.Domain.Models.Implements;
using Entity.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace test.Data
{
    public class RolUserRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly RolUserRepository _repository;

        public RolUserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new RolUserRepository(_context);

            // ==========================
            // Datos base de prueba
            // ==========================
            var rol1 = new Rol { Id = 1, Name = "Admin", Description="Description" };
            var rol2 = new Rol { Id = 2, Name = "User" , Description="Description"};

            var user1 = new User { Id = 1, Email = "user1@mail.com" };
            var user2 = new User { Id = 2, Email = "user2@mail.com" };

            _context.Rols.AddRange(rol1, rol2);
            _context.Users.AddRange(user1, user2);

            _context.RolUsers.AddRange(
                new RolUser { Id = 1, RolId = 1, UserId = 1, IsDeleted = false },
                new RolUser { Id = 2, RolId = 2, UserId = 2, IsDeleted = true },
                new RolUser { Id = 3, RolId = 1, UserId = 2, IsDeleted = false }
            );

            _context.SaveChanges();
        }

        // =============================================
        // TEST 1: GetAllAsync
        // =============================================
        [Fact]
        public async Task GetAllAsync_ShouldReturnActiveRolUsers_WithIncludes()
        {
            var result = await _repository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(r => !r.IsDeleted).Should().BeTrue();

            result.First().Rol.Should().NotBeNull();
            result.First().User.Should().NotBeNull();
        }

        // =============================================
        // TEST 2: GetDeletes
        // =============================================
        [Fact]
        public async Task GetDeletes_ShouldReturnDeletedRolUsers_WithIncludes()
        {
            var result = await _repository.GetDeletes();

            result.Should().ContainSingle(r => r.IsDeleted);
            result.First().Rol.Should().NotBeNull();
            result.First().User.Should().NotBeNull();
        }

        // =============================================
        // TEST 3: GetByIdAsync
        // =============================================
        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
        {
            var result = await _repository.GetByIdAsync(3);

            result.Should().NotBeNull();
            result!.Rol.Should().NotBeNull();
            result.User.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var result = await _repository.GetByIdAsync(999);
            result.Should().BeNull();
        }

        // =============================================
        // TEST 4: AsignateUserRol
        // =============================================
        [Fact]
        public async Task AsignateUserRol_ShouldAssignRoleId2ToUser()
        {
            var userId = 10;

            var assigned = await _repository.AsignateUserRol(userId);

            assigned.Should().NotBeNull();
            assigned.UserId.Should().Be(userId);
            assigned.RolId.Should().Be(2);
            assigned.Active.Should().BeTrue();
            assigned.IsDeleted.Should().BeFalse();

            _context.RolUsers.Should().ContainSingle(r => r.UserId == userId && r.RolId == 2);
        }

        // =============================================
        // TEST 5: GetRolesUserAsync
        // =============================================
        [Fact]
        public async Task GetRolesUserAsync_ShouldReturnRoleNames_ForUser()
        {
            var result = await _repository.GetRolesUserAsync(1);

            result.Should().NotBeNull();
            result.Should().ContainSingle(r => r == "Admin");
        }

        // =============================================
        // TEST 6: GetJoinRolesAsync
        // =============================================
        [Fact]
        public async Task GetJoinRolesAsync_ShouldReturnDistinctRoleNames_ForUser()
        {
            var result = await _repository.GetJoinRolesAsync(2);

            result.Should().NotBeNull();
            result.Should().ContainSingle(r => r == "Admin");
        }
    }
}
