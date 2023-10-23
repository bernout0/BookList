using BookListing.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.Reporting.Queries
{
    public class GetAuthorBookCounQuery : IRequest<List<AuthorBookCountDto>>
    {
    }

    public class GetAuthorBookCountQueryHandler : IRequestHandler<GetAuthorBookCounQuery, List<AuthorBookCountDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetAuthorBookCountQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuthorBookCountDto>> Handle(GetAuthorBookCounQuery request, CancellationToken cancellationToken)
        {
            var authorBookCounts = await _context.Books
                .GroupBy(b => b.Author)
                .Select(g => new AuthorBookCountDto
                {
                    Author = g.Key,
                    BookCount = g.Count()
                })
                .ToListAsync();

            return authorBookCounts;
        }
    }
}
