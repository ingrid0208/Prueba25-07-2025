using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Entity.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        ///<summary>
        ///Implementación DBSet
        ///</summary>
        public DbSet<User> users { get; set; }
        public DbSet<Rol> rols { get; set; }
        public DbSet<RolUser> rolUsers { get; set; }
        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Pizza> pizzas { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        public DbSet<DetallePedido> detallePedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Semilla de roles (ya estaba bien)
            modelBuilder.Entity<Rol>().HasData(
                new Rol { id = 1, name = "Admin", description = "Administrador del sistema" },
                new Rol { id = 2, name = "Asistente", description = "Asistente de pedidos" },
                new Rol { id = 3, name = "Pizzero", description = "Persona que prepara pizzas" }
            );

            // ✅ Solución al problema de cascadas múltiples:
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Restrict); // evita cascada

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict); // evita cascada

            // (opcional) aseguramos relaciones de DetallePedido
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.Cascade); // está bien dejarlo

            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Pizza)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.IdPizza)
                .OnDelete(DeleteBehavior.Restrict); // evita problema si es necesario
        }
    }
}

