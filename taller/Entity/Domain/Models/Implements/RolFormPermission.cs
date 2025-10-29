using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements
{
    public class RolFormPermission : BaseModel
    {
        public int RolId { get; set; }
        public int FormId { get; set; }
        public int PermissionId { get; set; }

        public Rol Rol { get; set; }
        public Form Form { get; set; }
        public Permission Permission { get; set; }
    }
}
