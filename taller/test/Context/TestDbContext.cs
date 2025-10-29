using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using test.Data;

namespace test.Context
{
    public class TestDbContext : ApplicationDbContext
    {
        public TestDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Registrar explícitamente la entidad de prueba
            modelBuilder.Entity<FakeEntity>();
        }

        public DbSet<FakeEntity> FakeEntities { get; set; } = null!;
    }

}
