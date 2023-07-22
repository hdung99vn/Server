using Core.Entities;

namespace API.Models
{
    public class Todo : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
