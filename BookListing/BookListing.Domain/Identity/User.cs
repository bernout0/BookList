using BookListing.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace BookListing.Domain.Identity
{
    public class User : IdentityUser
    {
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
        public virtual ICollection<UserAccess> UserAccesses { get; set; } = new List<UserAccess>();
    }
}
