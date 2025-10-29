using Entity.Domain.Interfaces;
using Entity.Domain.Models.Auth;
using Entity.Domain.Models.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Entity.Infrastructure.Contexts
{
    public class MySqlApplicationDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        private readonly IAuditService _auditService;
        private readonly IHttpContextAccessor _http;

        public MySqlApplicationDbContext(
             DbContextOptions<MySqlApplicationDbContext> options,
             IConfiguration configuration,
             IAuditService auditService,
             IHttpContextAccessor httpContextAccessor
         ) : base(options)
        {
            _configuration = configuration;
            _auditService = auditService;
            _http = httpContextAccessor;
        }

        public MySqlApplicationDbContext(DbContextOptions<MySqlApplicationDbContext> options) : base(options) { }


        ///<summary>
        ///Implementación DBSet
        ///</summary>

        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<RolUser> RolUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public DbSet<Form> Forms { get; set; }
        public DbSet<Domain.Models.Implements.Module> Modules { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolFormPermission> RolFormPermissions { get; set; }
        public DbSet<FormModule> FormModules { get; set; }
        //public DbSet<TouristicAttraction> TouristicAttraction { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Person)
                .WithOne(p => p.User)
                .HasForeignKey<User>(u => u.PersonId)
                .OnDelete(DeleteBehavior.Cascade); // o Restrict, según lo que desees



            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            // 1. Detecta cambios
            ChangeTracker.DetectChanges();
            // 2. Captura auditoría
            _auditService.CaptureAsync(ChangeTracker).GetAwaiter().GetResult();
            // 3. Persiste todo
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            await _auditService.CaptureAsync(ChangeTracker, cancellationToken);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
