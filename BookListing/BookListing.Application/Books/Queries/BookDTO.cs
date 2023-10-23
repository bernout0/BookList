using BookListing.Application.Categories.Queries;
using BookListing.Application.Common.Mappings;
using BookListing.Application.Departments.Queries;
using BookListing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.Books.Queries
{
    public class BookDTO : IMapFrom<Book>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        public CategoryDTO? Category { get; set; } = null!;

        public DepartmentDTO? Department { get; set; } = null!;
    }
}
