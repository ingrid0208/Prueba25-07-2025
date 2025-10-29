using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Domain.Models.Implements
{
    public class Rol : BaseModelGeneric
    {
        // Relación con RolUser
        public ICollection<RolUser> RolUsers { get; set; } = [];
    }
}
