using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Auth;

namespace Data.Interfaces.IDataImplement
{
    public interface IUserRepository: IData<User> 
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> FindEmail(string email);
        Task<bool> ExistsByDocumentAsync(string identification);
        Task<User> LoginUser(LoginUserDto loginDto);
    }
}
