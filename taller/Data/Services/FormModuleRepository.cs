using Data.Interfaces.IDataImplement;
using Data.Repositoy;
using Entity.Domain.Models.Implements;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    public class FormModuleRepository : DataGeneric<FormModule>, IFormModuleRepository
    {
        public FormModuleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<FormModule>> GetAllAsync()
        {
            return await _context.Set<FormModule>()
                        .Include(u => u.Form)
                        .Include(u => u.Module)
                        .Where(u => u.IsDeleted == false)
                        .ToListAsync();
        }

        public override async Task<IEnumerable<FormModule>> GetDeletes()
        {
            return await _context.Set<FormModule>()
                        .Include(u => u.Form)
                        .Include(u => u.Module)
                        .Where(u => u.IsDeleted == true)
                        .ToListAsync();
        }

        public override async Task<FormModule?> GetByIdAsync(int id)
        {
            return await _context.Set<FormModule>()
                      .Include(u => u.Form)
                      .Include(u => u.Module)
                      .Where(u => u.Id == id)
                      .FirstOrDefaultAsync();

        }
    }
}
