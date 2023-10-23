using AutoMapper;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Application.Departments.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.Reporting.Queries
{
    public class GetTotalNumberOfBooksQuery : IRequest<TotalBookCountDto>
    {
    }

    public class GetTotalNumberOfBooksQueryHandler : IRequestHandler<GetTotalNumberOfBooksQuery, TotalBookCountDto>
    {
        private readonly IApplicationDbContext _context;
        public GetTotalNumberOfBooksQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TotalBookCountDto> Handle(GetTotalNumberOfBooksQuery request, CancellationToken cancellationToken)
        {
            var count = await _context.Books.CountAsync();
            TotalBookCountDto totalBookCountDto = new TotalBookCountDto() { BookCount = count };

            return totalBookCountDto;
        }

    }
}
