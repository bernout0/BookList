using BookListing.Application.Common.Mappings;
using BookListing.Domain.Entities;

namespace BookListing.Application.Departments.Queries
{
    public class DepartmentDTO : IMapFrom<Department>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
