using Entity.Domain.Interfaces;
using Entity.DTOs.Base;

namespace Entity.DTOs.Default
{
    public class RolUserDto : BaseDto
    {
        public int UserId { get; set;}
        public int RolId {  get; set;}
    }
}
