using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Web.Service
{
    public static class DatabaseService
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var sql = configuration.GetConnectionString("SqlServer");
            var pg = configuration.GetConnectionString("Postgres");
            var my = configuration.GetConnectionString("MySql");   

            if (!string.IsNullOrWhiteSpace(sql))
            {
                services.AddDbContext<ApplicationDbContext>(opt =>
                    opt.UseSqlServer(sql, s =>
                    {
                        s.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        s.EnableRetryOnFailure();
                        s.CommandTimeout(60);
                    }));
            }

            if (!string.IsNullOrWhiteSpace(pg))
            {
                services.AddDbContext<PostgresDbContext>(opt =>
                    opt.UseNpgsql(pg, n =>
                    {
                        n.MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName);
                        n.EnableRetryOnFailure();
                        n.CommandTimeout(60);
                    }));
            }

            if (!string.IsNullOrWhiteSpace(my))
            {
                services.AddDbContext<MySqlApplicationDbContext>(opt =>
                    opt.UseMySql(my, ServerVersion.AutoDetect(my), m =>
                    {
                        m.MigrationsAssembly(typeof(MySqlApplicationDbContext).Assembly.FullName);
                        m.EnableStringComparisonTranslations();
                    })
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                );
            }

            return services;
        }
    }
}
