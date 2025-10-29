using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements
{
    public class Module : BaseModelGeneric
    {
      
        // Relación con FormModules
        public  ICollection<FormModule> FormModules { get; set; } = [];
    }
}
