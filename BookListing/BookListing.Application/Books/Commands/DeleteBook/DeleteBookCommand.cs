using AutoMapper;
using BookListing.Application.Books.Commands.UpdateBook;
using BookListing.Application.Common.Exceptions;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteBookCommandHandler : CategoryDepartmentAccessUtility, IRequestHandler<DeleteBookCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public DeleteBookCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Books
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Book), request.Id);
            }

            await ValidateUserAccess(entity);

            entity.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task ValidateUserAccess(Book request)
        {
            if (!await _identityService.IsAdmin(_currentUserService.UserId))
            {
                GetAllowedCategoriesDepartment(_context, _currentUserService,
                       out List<Guid> allowedCategoryIds, out List<Guid> allowedDepartmentIds);

                //Throw 401 if user has no access to the category
                if (!allowedCategoryIds.Contains(request.CategoryId.Value) && !allowedDepartmentIds.Contains(request.DepartmentId.Value))
                    throw new UnauthorizedAccessException();
            }
        }
    }
}
