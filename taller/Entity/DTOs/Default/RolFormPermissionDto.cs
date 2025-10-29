using Entity.Domain.Interfaces;
using Entity.DTOs.Base;

namespace Entity.DTOs.Default
{
    public class RolFormPermissionDto : BaseDto
    {
        public int RolId { get; set; }
        public int FormId { get; set; }
        public int PermissionId { get; set; }
    }
}
