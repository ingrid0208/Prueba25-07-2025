namespace Entity.Domain.Models.Base
{
    public class BaseModel
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
