using BookListing.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookListing.Application.UserAccess.Commands.CreateUserAccess
{
    public class CreateUserAccessCommandValidator : AbstractValidator<CreateUserAccessCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserAccessCommandValidator(IApplicationDbContext context)
        {
            _context = context;


            //Make sure User ID is not empty
            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("User Id is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");


            RuleFor(v => v.CategoryId)
              .NotEmpty().When(v => string.IsNullOrEmpty(v.DepartmentId.ToString()))
              .WithMessage("Category is required when Department is not provided.");

            RuleFor(v => v.DepartmentId)
                .NotEmpty().When(v => string.IsNullOrEmpty(v.CategoryId.ToString()))
                .WithMessage("Department is required when Category is not provided.");

            RuleFor(v => new { v.CategoryId, v.DepartmentId })
                .Must(x => !(x.CategoryId != null && x.DepartmentId != null))
                .WithMessage("You cannot provide both Category and Department. Choose one.");

            //Make sure it's a unique combination of user access
            RuleFor(v => v)
                .MustAsync(BeUniqueCombination).WithMessage("Access control already exists");
        }


        public async Task<bool> BeUniqueCombination(CreateUserAccessCommand command, CancellationToken cancellationToken)
        {
            // Check if there's any existing combination that matches the provided one
            return !await _context.UserAccesses.AnyAsync(l =>
            l.UserId == command.UserId &&
            l.DepartmentId == command.DepartmentId &&
            l.CategoryId == command.CategoryId,
            cancellationToken);
        }


    }



}
