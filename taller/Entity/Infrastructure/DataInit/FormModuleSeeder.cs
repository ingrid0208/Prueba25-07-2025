using Entity.Domain.Models.Implements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    internal class FormModuleSeeder : IEntityTypeConfiguration<FormModule>
    {
        public void Configure(EntityTypeBuilder<FormModule> builder)
        {
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                new FormModule
                { 
                    Id = 1,
                    FormId = 1,
                    ModuleId = 1,
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date
                },
                new FormModule
                {
                    Id = 2,
                    FormId = 2,
                    ModuleId = 2,
                    Active = true,
                    IsDeleted = false,
                    CreatedAt = date

                }
            );
        }
    }
}
