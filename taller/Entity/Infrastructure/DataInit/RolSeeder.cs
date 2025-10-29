using Entity.Domain.Models.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    public class RolSeeder : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.Property(u => u.Name)
                .HasColumnType("varchar(100)");

            builder.Property(u => u.Description)
                .HasColumnType("varchar(100)");

            builder.HasData(
                new Rol
                {
                    Id = 1,
                    Name = "Administrador",
                    Description = "Rol con permisos administrativos",
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date
                },
                new Rol
                {
                    Id = 2,
                    Name = "Usuario",
                    Description = "Rol con permisos de usuario",
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date

                }
            );
        }
    }
}
