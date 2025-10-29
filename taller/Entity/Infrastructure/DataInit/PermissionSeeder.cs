using Entity.Domain.Models.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    public class PermissionSeeder : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();
            var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.Property(u => u.Name)
                .HasColumnType("varchar(100)");

            builder.Property(u => u.Description)
                .HasColumnType("varchar(100)");

            builder.HasData(
                new Permission
                {
                    Id = 1,
                    Name = "Crear",
                    Description = "Permiso de creacion",
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date
                },
                new Permission
                {
                    Id = 2,
                    Name = "Borrar",
                    Description = "Permiso de borrar",
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date
                },
                new Permission
                {
                    Id = 3,
                    Name = "Actualizar",
                    Description = "Permiso de Actualizar",
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date
                },
                new Permission
                {
                    Id = 4,
                    Name = "Leer",
                    Description = "Permiso de Leer",
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date
                }

            );
        }
    }
}
