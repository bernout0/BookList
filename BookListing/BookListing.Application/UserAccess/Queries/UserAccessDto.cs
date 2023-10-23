using BookListing.Application.Categories.Queries;
using BookListing.Application.Common.Mappings;
using BookListing.Application.Departments.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.UserAccess.Queries
{
    public class UserAccessDto : IMapFrom<BookListing.Domain.Identity.UserAccess>
    {
        public Guid Id { get; set; }

        public CategoryDTO? Category { get; set; } = null!;
        public DepartmentDTO? Department { get; set; } = null!;

    }
}
