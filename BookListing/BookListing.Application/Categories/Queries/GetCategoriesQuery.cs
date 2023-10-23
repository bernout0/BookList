using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookListing.Application.Books.Queries;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.Categories.Queries
{
    public class GetCategoriesQuery : IRequest<List<CategoryDTO>>
    {
    }
    public class GetCategoriesQueryHandler : CategoryDepartmentAccessUtility, IRequestHandler<GetCategoriesQuery, List<CategoryDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<List<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            //Return all books if user is admin else filter by books he has access to
            if (await _identityService.IsAdmin(_currentUserService.UserId))
                return await _context.Categories
             .OrderBy(x => x.Name)
             .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider).ToListAsync();
            else
            {
                GetAllowedCategoriesDepartment(_context, _currentUserService,
                        out List<Guid> allowedCategoryIds, out List<Guid> allowedDepartmentIds);

                // Return books that match the categories and departments.
                return await _context.Categories
                    .Where(b => allowedCategoryIds.Contains(b.Id))
                    .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }

            
        }
    }
}
