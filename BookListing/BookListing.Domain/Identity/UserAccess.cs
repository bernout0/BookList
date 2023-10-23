using BookListing.Domain.Entities;

namespace BookListing.Domain.Identity
{
    public class UserAccess
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public Guid? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public Guid? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
    }
}
