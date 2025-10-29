using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements
{
    public class Person : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public User User { get; set; }
    }

}
