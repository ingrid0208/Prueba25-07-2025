using Entity.DTOs.Base;

namespace Entity.DTOs.Default
{
    public class PersonDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
