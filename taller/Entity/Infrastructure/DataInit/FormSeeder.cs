using Entity.Domain.Models.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    public class FormSeeder : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);


            builder.Property(u => u.Name)
                .HasColumnType("varchar(100)");

            builder.Property(u => u.Description)
                .HasColumnType("varchar(100)");

            builder.HasData(
                new Form
                {
                    Id = 1,
                    Name = "Form User",
                    Description = "Form Update User",
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date
                },
                new Form
                {
                    Id = 2,
                    Name = "Form Rol",
                    Description = "Form Create Rol",
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date

                }
            );
        }
    }
}
