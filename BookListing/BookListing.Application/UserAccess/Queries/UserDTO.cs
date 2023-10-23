using BookListing.Application.Common.Mappings;
using BookListing.Domain.Entities;
using BookListing.Domain.Identity;

namespace BookListing.Application.UserAccess.Queries
{
    public class UserDTO : IMapFrom<User>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public List<UserAccessDto> UserAccesses { get; set; }

    }
}
