using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements
{
    public class FormModule : BaseModel
    {
       
        public int FormId { get; set; }
        public int ModuleId { get; set; }

        public Form Form { get; set; }

        public Module Module { get; set; }
    }
}
