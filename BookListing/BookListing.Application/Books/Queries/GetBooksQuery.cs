using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookListing.Application.Categories.Queries;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookListing.Application.Books.Queries
{
    public class GetBooksQuery : IRequest<List<BookDTO>>
    {
    }
    public class GetBooksQueryHandler : CategoryDepartmentAccessUtility, IRequestHandler<GetBooksQuery, List<BookDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public GetBooksQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<List<BookDTO>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            //Return all books if user is admin else filter by books he has access to
            if (await _identityService.IsAdmin(_currentUserService.UserId))
                return await _context.Books
                    .Where(i=>!i.IsDeleted)
              .OrderBy(x => x.Created)
              .ProjectTo<BookDTO>(_mapper.ConfigurationProvider).ToListAsync();
            else
            {
                GetAllowedCategoriesDepartment(_context, _currentUserService,
                        out List<Guid> allowedCategoryIds, out List<Guid> allowedDepartmentIds);

                // Return books that match the categories and departments.
                return await _context.Books
                    .Where(b => (allowedCategoryIds.Contains(b.CategoryId.Value) || allowedDepartmentIds.Contains(b.DepartmentId.Value) && !b.IsDeleted))
                    .OrderBy(x => x.Created)
                    .ProjectTo<BookDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }

        }
    }


}
