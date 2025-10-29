using Data.Repositoy;
using Entity.Domain.Models.Base;
using Entity.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using test.Context;

namespace test.Data
{
    // Entidad de prueba simple
    public class FakeEntity : BaseModel
    {
        public string? Name { get; set; }
    }

    public class DataGenericTests
    {
        private readonly ApplicationDbContext _context;
        private readonly DataGeneric<FakeEntity> _repository;

        public DataGenericTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);
            _repository = new DataGeneric<FakeEntity>(_context);

            // Datos iniciales
            _context.Set<FakeEntity>().AddRange(
                new FakeEntity { Id = 1, Name = "A", IsDeleted = false },
                new FakeEntity { Id = 2, Name = "B", IsDeleted = true },
                new FakeEntity { Id = 3, Name = "C", IsDeleted = false }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnOnlyActive()
        {
            var result = await _repository.GetAllAsync();

            result.Should().HaveCount(2);
            result.All(x => !x.IsDeleted).Should().BeTrue();
        }

        [Fact]
        public async Task GetDeletesShouldReturnOnlyDeleted()
        {
            var result = await _repository.GetDeletes();

            result.Should().ContainSingle(x => x.IsDeleted);
            result.First().Id.Should().Be(2);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnEntityWhenExistsAndActive()
        {
            var result = await _repository.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("A");
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullWhenDeleted()
        {
            var result = await _repository.GetByIdAsync(2);
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsyncShouldAddNewEntity()
        {
            var entity = new FakeEntity { Id = 4, Name = "D" };

            var created = await _repository.CreateAsync(entity);

            created.Should().NotBeNull();
            _context.Set<FakeEntity>().Count().Should().Be(4);
        }

        [Fact]
        public async Task UpdateAsyncShouldModifyExistingEntity()
        {
            var entity = _context.Set<FakeEntity>().First(x => x.Id == 1);
            entity.Name = "Updated";

            var result = await _repository.UpdateAsync(entity);

            result.Should().BeTrue();

            var updated = await _repository.GetByIdAsync(1);
            updated!.Name.Should().Be("Updated");
        }

        [Fact]
        public async Task DeleteAsyncShouldRemoveEntity()
        {
            var result = await _repository.DeleteAsync(1);

            result.Should().BeTrue();
            _context.Set<FakeEntity>().Find(1).Should().BeNull();
        }

        [Fact]
        public async Task DeleteLogicAsyncShouldMarkEntityAsDeleted()
        {
            var result = await _repository.DeleteLogicAsync(3);

            result.Should().BeTrue();

            var entity = await _repository.GetByIdAsync(3);
            entity.Should().BeNull();

            var deleted = await _context.Set<FakeEntity>().FindAsync(3);
            deleted!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task RestoreAsyncShouldUnmarkEntityAsDeleted()
        {
            var entity = await _context.Set<FakeEntity>().FindAsync(2);
            entity!.IsDeleted.Should().BeTrue();

            var result = await _repository.RestoreAsync(2);

            result.Should().BeTrue();

            var restored = await _context.Set<FakeEntity>().FindAsync(2);
            restored!.IsDeleted.Should().BeFalse();
        }
    }
}
