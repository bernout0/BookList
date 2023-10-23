using AutoMapper;
using BookListing.Application.Categories.Queries;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.Departments.Queries
{
    public class GetDepartmentByIdQuery
    {
        public class Query : IRequest<DepartmentDTO>
        {
            public Guid Id { get; set; }
        }

        public class GetDepartmentByQueryHandler : CategoryDepartmentAccessUtility, IRequestHandler<Query, DepartmentDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;
            private readonly IIdentityService _identityService;

            public GetDepartmentByQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, ICurrentUserService currentUserService)
            {
                _context = context;
                _mapper = mapper;
                _currentUserService = currentUserService;
                _identityService = identityService;
            }


            public async Task<DepartmentDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                Department entity = await _context.Departments.FindAsync(new object[] { request.Id }, cancellationToken);

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

                var viewModel = _mapper.Map<DepartmentDTO>(entity);
                return viewModel;
            }
        }
    }
}
