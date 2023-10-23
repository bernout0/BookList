using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookListing.Application.Categories.Queries;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;

namespace BookListing.Application.Categories.Queries
{
    public class GetCategoryByIdQuery
    {
        public class Query : IRequest<CategoryDTO>
        {
            public Guid Id { get; set; }
        }

        public class GetCategoryByQueryHandler : CategoryDepartmentAccessUtility, IRequestHandler<Query, CategoryDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IIdentityService _identityService;
            private readonly ICurrentUserService _currentUserService;


            public GetCategoryByQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, ICurrentUserService currentUserService)
            {
                _context = context;
                _mapper = mapper;
                _identityService = identityService;
                _currentUserService = currentUserService;
            }



            public async Task<CategoryDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                Category entity = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

                if (entity is null)
                    throw new KeyNotFoundException($"Category record for key {request.Id} not found.");

                if (!await _identityService.IsAdmin(_currentUserService.UserId)) //Normal user
                {
                    GetAllowedCategoriesDepartment(_context, _currentUserService,
                        out List<Guid> allowedCategoryIds, out List<Guid> allowedDepartmentIds);

                    //Throw 401 if user has no access to the category
                    if (!allowedCategoryIds.Contains(entity.Id))
                        throw new UnauthorizedAccessException();
                }

                var viewModel = _mapper.Map<CategoryDTO>(entity);
                return viewModel;
            }
        }
    }
}
