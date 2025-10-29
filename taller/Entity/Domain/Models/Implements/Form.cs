using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements
{
    public class Form : BaseModelGeneric
    {
        // Relaciones
        public ICollection<RolFormPermission> RolFormPermissions { get; set; } = [];

        public ICollection<FormModule> FormModules { get; set; } = [];
    }
}
