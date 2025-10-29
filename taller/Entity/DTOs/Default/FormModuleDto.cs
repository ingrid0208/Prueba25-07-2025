using Entity.Domain.Interfaces;
using Entity.DTOs.Base;

namespace Entity.DTOs.Default
{
    public class FormModuleDto : BaseDto
    {
        public int FormId { get; set; }
        public int ModuleId { get; set; }
    }
}
