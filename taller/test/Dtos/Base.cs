using Entity.Domain.Models.Base;

namespace test.Dtos
{
    public class FakeEntity : BaseModel
    {
        public string? Name { get; set; }
    }

    public class FakeCreateDto
    {
        public string? Name { get; set; }
    }

    public class FakeSelectDto
    {
        public string? Name { get; set; }
    }
}
