using AutoMapper;
using BookListing.Application.Categories.Queries;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookListing.Application.Books.Queries
{
    public class GetBookByIdQuery
    {
        public class Query : IRequest<BookDTO>
        {
            public Guid Id { get; set; }
        }

        public class GetBookByIdQueryHandler : CategoryDepartmentAccessUtility, IRequestHandler<Query, BookDTO>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IIdentityService _identityService;
            private readonly ICurrentUserService _currentUserService;

            public GetBookByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, ICurrentUserService currentUserService)
            {
                _context = context;
                _mapper = mapper;
                _identityService = identityService;
                _currentUserService = currentUserService;
            }

            public async Task<BookDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                Book entity;

                entity = await _context.Books.FindAsync(new object[] { request.Id }, cancellationToken);
                             
                if (entity is null)
                    throw new KeyNotFoundException($"Book record for key {request.Id} not found.");

                if (!await _identityService.IsAdmin(_currentUserService.UserId)) //Normal user
                {
                    GetAllowedCategoriesDepartment(_context, _currentUserService,
                        out List<Guid> allowedCategoryIds, out List<Guid> allowedDepartmentIds);

                    //Throw 401 if user has no access to the book
                    if (!allowedCategoryIds.Contains(entity.CategoryId.Value) && !allowedDepartmentIds.Contains(entity.DepartmentId.Value))
                        throw new UnauthorizedAccessException();
                }

                var viewModel = _mapper.Map<BookDTO>(entity);
                return viewModel;
            }

        }
    }
}
