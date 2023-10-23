using AutoMapper;
using BookListing.Application.Books.Queries;
using BookListing.Application.Categories.Queries;
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

namespace BookListing.Application.Books.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<BookDTO>
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public Guid? CategoryId { get; set; }
        public Guid? DepartmentId { get; set; }

    }

    public class CreateBookCommandHandler : CategoryDepartmentAccessUtility, IRequestHandler<CreateBookCommand, BookDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;


        public CreateBookCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<BookDTO> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            await ValidateUserAccess(request);

            var entity = new Book()
            {
                Title = request.Title,
                Description = request.Description,
                Author = request.Author,
                CategoryId = request.CategoryId,
                DepartmentId = request.DepartmentId
            };

            _context.Books.Add(entity);

            //TODO: ADD CREATED EVENT


            await _context.SaveChangesAsync(cancellationToken);

            var viewModel = _mapper.Map<BookDTO>(entity);

            return viewModel;
        }


        private async Task ValidateUserAccess(CreateBookCommand request)
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
