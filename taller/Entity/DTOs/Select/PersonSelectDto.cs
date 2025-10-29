using Entity.DTOs.Base;

namespace Entity.DTOs.Select
{
    public class PersonSelectDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
