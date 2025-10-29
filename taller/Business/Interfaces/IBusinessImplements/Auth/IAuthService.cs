using Entity.DTOs.Auth;
using Entity.DTOs.Default;

namespace Business.Interfaces.IBusinessImplements.Auth
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto dto);
    }
}
