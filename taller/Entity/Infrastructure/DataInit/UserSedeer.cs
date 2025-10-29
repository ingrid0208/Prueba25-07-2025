using Entity.Domain.Models.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    public class UserSedeer : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);



            builder.Property(u => u.Password)
                .HasColumnType("varchar(100)");

            builder.Property(u => u.Email)
                .HasColumnType("varchar(150)");

            builder.HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@example.com",
                    Password = "admin123",
                    Active = true,
                    IsDeleted = false,
                    PersonId = 1,
                    CreatedAt =date
                },
                new User
                {
                    Id = 2,
                    Email = "User@example.com",
                    Password = "user123",
                    Active = true,
                    IsDeleted = false,
                    PersonId = 2,
                    CreatedAt = date
                }

            );
        }
    }
}
