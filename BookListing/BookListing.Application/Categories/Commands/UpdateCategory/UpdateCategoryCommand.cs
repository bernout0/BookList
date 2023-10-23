using BookListing.Application.Common.Exceptions;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookListing.Application.Categories.Commands.UpdateCategory
{
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "DepartmentsManagement")]
    public class UpdateCategoryCommand : IRequest
    {
        [Required(ErrorMessage = "CategoryId is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }


    }

    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTodoListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {

            var entity = await _context.Categories
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }


            entity.Name = request.Name;


            //TODO: ADD CREATED EVENT

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
