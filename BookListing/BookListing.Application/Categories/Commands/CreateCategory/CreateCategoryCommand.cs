using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookListing.Application.Categories.Commands.CreateCategory
{
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "DepartmentsManagement")]
    public class CreateCategoryCommand : IRequest<Category>
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }      

    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateCategoryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Category()
            {
                Name = request.Name
            };

            _context.Categories.Add(entity);


            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
