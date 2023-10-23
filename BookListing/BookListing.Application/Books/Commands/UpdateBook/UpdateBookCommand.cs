using AutoMapper;
using BookListing.Application.Books.Commands.CreateBook;
using BookListing.Application.Common.Exceptions;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest
    {
        [Required(ErrorMessage = "BookId is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public Guid? CategoryId { get; set; }
        public Guid? DepartmentId { get; set; }
    }

    public class UpdateTodoListCommandHandler : CategoryDepartmentAccessUtility, IRequestHandler<UpdateBookCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public UpdateTodoListCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            await ValidateUserAccess(request);

            var entity = await _context.Books
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Book), request.Id);
            }
            

            entity.Title = request.Title;
            entity.Author = request.Author;
            entity.Description = request.Description;
            entity.CategoryId = request.CategoryId;
            entity.DepartmentId = request.DepartmentId;


            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task UserHasAccessToDepartment(CreateBookCommand request)
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

        private async Task ValidateUserAccess(UpdateBookCommand request)
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
