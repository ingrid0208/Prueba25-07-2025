using Data.Interfaces.IDataImplement;
using Data.Repositoy;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Auth;
using Entity.DTOs.Default;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Utilities.Custom;

namespace Data.Services
{
    public class UserRepository: DataGeneric<User>, IUserRepository
    {

        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }



        public async Task<User?> FindEmail(string email)
        {
            var user = await _dbSet.Where(u => u.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && u.IsDeleted == false);
        }
        public async Task<bool> ExistsByDocumentAsync(string identification)
        {
            return await _dbSet.AnyAsync(u => u.Person.Identification == identification);
        }

        public async Task<User> LoginUser(LoginUserDto loginDto)
        {
            bool suceeded = false;

            var user = await _dbSet
                .FirstOrDefaultAsync(u =>
                            u.Email == loginDto.Email &&
                            u.Password == loginDto.Password);

            suceeded = user != null ? true : throw new UnauthorizedAccessException("Credenciales inválidas");

            return user;
        }
    }
}
