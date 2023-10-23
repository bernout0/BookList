using BookListing.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.Reporting.Queries
{
    public class GetDepartmentBookCountQuery : IRequest<List<DepartmentBookCountDto>>
    {
    }

    public class GetDepartmentBookCountQueryHandler : IRequestHandler<GetDepartmentBookCountQuery, List<DepartmentBookCountDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetDepartmentBookCountQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<DepartmentBookCountDto>> Handle(GetDepartmentBookCountQuery request, CancellationToken cancellationToken)
        {

            var authorBookCounts = await _context.Books
                .GroupBy(b => b.Department.Name)
                .Select(g => new DepartmentBookCountDto
                {
                    DepartmentName = g.Key ?? "No Department",
                    BookCount = g.Count()
                })
                .ToListAsync();

            return authorBookCounts;
        }
    }
}
