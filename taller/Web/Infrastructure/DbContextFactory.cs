namespace Web.Infrastructure
{
    using Entity.Domain.Enums;
    using Entity.Infrastructure.Contexts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    namespace Web.Infrastructure
    {
        public class DbContextFactory
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IConfiguration _configuration;

            public DbContextFactory(
                IHttpContextAccessor httpContextAccessor,
                IConfiguration configuration)
            {
                _httpContextAccessor = httpContextAccessor;
                _configuration = configuration;
            }

            public ApplicationDbContext CreateDbContext()
            {
                var provider = _httpContextAccessor.HttpContext?.Items["DbProvider"] as DatabaseType?
                               ?? DatabaseType.SqlServer;

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                switch (provider)
                {
                    case DatabaseType.postgres:
                        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSql"));
                        break;

                    case DatabaseType.MySql:
                        var conn = _configuration.GetConnectionString("MySql");
                        optionsBuilder.UseMySql(conn, ServerVersion.AutoDetect(conn));
                        break;

                    case DatabaseType.SqlServer:
                    default:
                        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServer"));
                        break;
                }

                return new ApplicationDbContext(optionsBuilder.Options, _configuration, _httpContextAccessor);
            }
        }
    }

}
