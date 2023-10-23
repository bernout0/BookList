using BookListing.Application.Common.Mappings;
using BookListing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.Categories.Queries
{
    public class CategoryDTO : IMapFrom<Category>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
