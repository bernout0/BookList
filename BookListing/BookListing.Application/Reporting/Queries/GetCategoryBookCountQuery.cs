using BookListing.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.Reporting.Queries
{
    public class GetCategoryBookCountQuery : IRequest<List<CategoryBookCountDto>>
    {
    }

    public class GetCategoryBookCountQueryHandler : IRequestHandler<GetCategoryBookCountQuery, List<CategoryBookCountDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetCategoryBookCountQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<CategoryBookCountDto>> Handle(GetCategoryBookCountQuery request, CancellationToken cancellationToken)
        {

            var authorBookCounts = await _context.Books
                .GroupBy(b => b.Category.Name)
                .Select(g => new CategoryBookCountDto
                {
                    CategoryName = g.Key ?? "No Category",
                    BookCount = g.Count()
                })
                .ToListAsync();

            return authorBookCounts;
        }
    }
}
