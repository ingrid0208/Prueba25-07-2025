using Entity.DTOs.Base;

namespace Entity.DTOs.Default
{
    public class UserDto : BaseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public int? PersonId { get; set; }

    }
}
