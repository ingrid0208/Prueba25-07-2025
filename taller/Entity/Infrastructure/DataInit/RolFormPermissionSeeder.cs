using Entity.Domain.Models.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    public class RolFormPermissionSeeder : IEntityTypeConfiguration<RolFormPermission>
    {
        public void Configure(EntityTypeBuilder<RolFormPermission> builder)
        {
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);


            builder.HasData(
                new RolFormPermission
                {
                    Id = 1,
                    RolId = 1,
                    FormId = 1,
                    PermissionId = 1,
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date
                },
                new RolFormPermission
                {
                    Id = 2,
                    RolId = 2,
                    FormId = 2,
                    PermissionId = 2,
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date

                }
            );
        }
    }
}
