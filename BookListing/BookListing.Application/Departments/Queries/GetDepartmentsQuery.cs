using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookListing.Application.Categories.Queries;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.Departments.Queries
{
    public class GetDepartmentsQuery : IRequest<List<DepartmentDTO>>
    {
    }
    public class GetDepartmentsQueryHandler : CategoryDepartmentAccessUtility, IRequestHandler<GetDepartmentsQuery, List<DepartmentDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public GetDepartmentsQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }
        public async Task<List<DepartmentDTO>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
        {
            //Return all books if user is admin else filter by books he has access to
            if (await _identityService.IsAdmin(_currentUserService.UserId))
                return await _context.Departments
                 .OrderBy(x => x.Name)
                 .ProjectTo<DepartmentDTO>(_mapper.ConfigurationProvider).ToListAsync();
            else
            {
                GetAllowedCategoriesDepartment(_context, _currentUserService,
                        out List<Guid> allowedCategoryIds, out List<Guid> allowedDepartmentIds);

                // Return books that match the categories and departments.
                return await _context.Departments
                    .Where(b => allowedDepartmentIds.Contains(b.Id))
                    .ProjectTo<DepartmentDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }


        }
    }
}
