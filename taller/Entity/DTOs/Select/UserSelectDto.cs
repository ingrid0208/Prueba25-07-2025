using Entity.DTOs.Base;

namespace Entity.DTOs.Select
{
    public class UserSelectDto : BaseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
