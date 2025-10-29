using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements
{
    public class Permission : BaseModelGeneric
    {

        public ICollection<RolFormPermission> RolFormPermissions { get; set; } = [];
    }
}
