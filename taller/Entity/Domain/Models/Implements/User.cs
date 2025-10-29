using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements
{
    public class User : BaseModel
    {
        public string? Password { get; set; } 
        public string Email { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public ICollection<RolUser> RolUsers { get; set; } = [];
    }

}
