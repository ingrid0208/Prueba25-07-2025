using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements;

public class RolUser : BaseModel
{
    public int UserId { get; set; }

    public int RolId { get; set; }
    public User User { get; set; }

    public Rol Rol { get; set; }
}
