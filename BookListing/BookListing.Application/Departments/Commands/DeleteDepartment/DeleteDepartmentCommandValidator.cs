using BookListing.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteDepartmentCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Id)
                        .MustAsync(NotBeAttachedToABook)
                        .WithMessage("Department is attached to one or more books and cannot be deleted.");

        }

        private async Task<bool> NotBeAttachedToABook(Guid DepartmentId, CancellationToken cancellationToken)
        {
            return !await _context.Books.AnyAsync(b => b.DepartmentId == DepartmentId, cancellationToken);
        }
    }
}
