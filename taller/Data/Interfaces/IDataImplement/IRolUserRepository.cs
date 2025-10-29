using Data.Interfaces.DataBasic;

namespace Data.Interfaces.IDataImplement
{
    public interface IRolUserRepository : IData<RolUser>
    {
        Task<IEnumerable<string>> GetJoinRolesAsync(int userId);
        Task<RolUser> AsignateUserRol(int userId);
        Task<IEnumerable<string>> GetRolesUserAsync(int userId);

    }
}
