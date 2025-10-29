using Entity.Domain.Models.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    public class RolUserSeeder : IEntityTypeConfiguration<RolUser>
    {
        public void Configure(EntityTypeBuilder<RolUser> builder)
        {
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new RolUser
                {
                    Id = 1,
                    UserId = 1,
                    RolId = 1,
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date
                }
            );
        }
    }
}
