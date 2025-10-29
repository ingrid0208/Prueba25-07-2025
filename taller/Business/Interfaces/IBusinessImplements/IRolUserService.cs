using Business.Interfaces.BusinessBasic;
using Entity.DTOs.Default;
using Entity.DTOs.Select;

namespace Business.Interfaces.IBusinessImplements
{
    public interface IRolUserService : IBusiness<RolUserSelectDto,RolUserDto>
    {

        Task<IEnumerable<string>> GetAllRolUser(int idUser);
        Task<RolUserDto> AsignateUserRTo(int userId);
    }
}
