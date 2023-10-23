using BookListing.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandValidator: AbstractValidator<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Id)
                        .MustAsync(NotBeAttachedToABook)
                        .WithMessage("Category is attached to one or more books and cannot be deleted.");

        }

        private async Task<bool> NotBeAttachedToABook(Guid categoryId, CancellationToken cancellationToken)
        {
            return !await _context.Books.AnyAsync(b => b.CategoryId == categoryId, cancellationToken);
        }
    }
}
