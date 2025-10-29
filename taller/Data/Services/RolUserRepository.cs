using Data.Interfaces.IDataImplement;
using Data.Repositoy;
using Entity.Domain.Models.Implements;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Data.Services
{
    public class RolUserRepository : DataGeneric<RolUser>, IRolUserRepository
    {
        public RolUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<RolUser> AsignateUserRol(int userId)
        {
            var rolUser = new RolUser
            {
                UserId = userId,
                RolId = 2,
                Active = true,
                IsDeleted = false
            };

            _dbSet.Add(rolUser);
            await _context.SaveChangesAsync();

            return rolUser;
        }


        public override async Task<IEnumerable<RolUser>> GetAllAsync()
        {
            return await _context.Set<RolUser>()
                        .Include(u => u.Rol)
                        .Include(u => u.User)
                        .Where(u => u.IsDeleted == false)
                        .ToListAsync();
        }

        public override async Task<IEnumerable<RolUser>> GetDeletes()
        {
            return await _context.Set<RolUser>()
                        .Include(u => u.Rol)
                        .Include(u => u.User)
                        .Where(u => u.IsDeleted == true)
                        .ToListAsync();
        }


        public override async Task<RolUser?> GetByIdAsync(int id)
        {
            return await _context.Set<RolUser>()
                      .Include(u => u.Rol)
                      .Include(u => u.User)
                      .Where(u => u.Id == id)
                      .FirstOrDefaultAsync(u => u.IsDeleted == false);   

        }

        public async Task<IEnumerable<string>> GetRolesUserAsync(int userId)
        {
            var roles = await _dbSet
                    .Where(ru => ru.UserId == userId && !string.IsNullOrWhiteSpace(ru.Rol.Name))
                    .Select(ru => ru.Rol.Name)
                    .Distinct()
                    .ToListAsync();

            return roles;
        }

        public async Task<IEnumerable<string>> GetJoinRolesAsync(int idUser)
        {
            var rolAsignated = await _context.Set<RolUser>()
                               .Include(ru => ru.Rol)
                               .Include(ru => ru.User)
                               .Where(ru => ru.UserId == idUser)
                               .ToListAsync();

            var roles = rolAsignated
                                .Select(ru => ru.Rol.Name)
                                .Where(name => !string.IsNullOrWhiteSpace(name))
                                .Distinct()
                                .ToList();
            return roles;
        }
    }
}
