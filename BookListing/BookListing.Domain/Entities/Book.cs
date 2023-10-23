using BookListing.Domain.Common;
using BookListing.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Domain.Entities
{
    public class Book: AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; } = null!;

        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
      
    }

}
