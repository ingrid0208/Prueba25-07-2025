using Business.Interfaces.BusinessBasic;
using Entity.Domain.Models.Implements;
using Entity.DTOs.Default;
using Entity.DTOs.Select;

namespace Business.Interfaces.IBusinessImplements
{
    public interface IUserService : IBusiness<UserSelectDto,UserDto>
    {
        Task<UserDto> CreateAsyncUser(UserDto dto); 
    }
}
