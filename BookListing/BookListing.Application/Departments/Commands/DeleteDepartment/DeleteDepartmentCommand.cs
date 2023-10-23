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

namespace BookListing.Application.Departments.Commands.DeleteDepartment
{
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "DepartmentsManagement")]
    public class DeleteDepartmentCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteDepartmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {

            var entity = await _context.Departments
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Department), request.Id);
            }

            _context.Departments.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
